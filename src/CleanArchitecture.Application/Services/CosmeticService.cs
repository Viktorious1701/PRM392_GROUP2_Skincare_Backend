// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/Services/CosmeticService.cs
using CleanArchitecture.Application.Constants;
using CleanArchitecture.Application.DTOs.AzureBlob;
using CleanArchitecture.Application.DTOs.BatchDto;
using CleanArchitecture.Application.DTOs.BrandDto;
using CleanArchitecture.Application.DTOs.Cosmetic;
using CleanArchitecture.Application.DTOs.CosmeticImageDto;
using CleanArchitecture.Application.DTOs.CosmeticSubcategory;
using CleanArchitecture.Application.DTOs.CosmeticTypeDto;
using CleanArchitecture.Application.DTOs.FeedbackDto;
using CleanArchitecture.Application.DTOs.SkinTypeDto;
using CleanArchitecture.Application.DTOs.Store;
using CleanArchitecture.Application.Factories.FilePathFactory;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Strategies.BlogFilterStrategy;
using CleanArchitecture.Application.Strategies.CosmeticsFilterStrategy;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Services
{
  public class CosmeticService : ICosmeticService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorFactory _errorFactory;
    private readonly IBlobService _blobService;
    private readonly IValidator<CosmeticImagesUploadRequest> _cosmeticImagesUploadValidator;
    private readonly IFilePathFactory _filePathFactory;
    private readonly IEnumerable<ICosmeticFilterStrategy> _filterStrategies;

    public CosmeticService(
      IUnitOfWork unitOfWork,
      IErrorFactory errorFactory,
      IBlobService blobService,
      IValidator<CosmeticImagesUploadRequest> cosmeticImagesUploadValidator,
      IFilePathFactory filePathFactory,
      IEnumerable<ICosmeticFilterStrategy> filterStrategies
    )
    {
      _unitOfWork = unitOfWork;
      _errorFactory = errorFactory;
      _blobService = blobService;
      _cosmeticImagesUploadValidator = cosmeticImagesUploadValidator;
      _filePathFactory = filePathFactory;
      _filterStrategies = filterStrategies;
    }

    public async Task<Result<CosmeticResponse>> CreateCosmetic(CreateCosmetic request)
    {
      try
      {
        // Create and set up the Cosmetic entity
        var orgcosmetic = request.Adapt<Cosmetic>();
        orgcosmetic.BrandId = request.BrandId;
        orgcosmetic.SkinTypeId = request.SkinTypeId;
        orgcosmetic.CosmeticTypeId = request.CosmeticTypeId;
        orgcosmetic.StoreId = request.StoreId; // Assign StoreId

        // Attach existing related entities to avoid re-adding them
        orgcosmetic.Brand = new Brand { Id = request.BrandId };
        orgcosmetic.SkinType = new SkinType { Id = request.SkinTypeId };
        orgcosmetic.CosmeticType = new CosmeticType { Id = request.CosmeticTypeId };

        // Only attach store if StoreId is provided
        if (request.StoreId.HasValue)
        {
          orgcosmetic.Store = new Store { Id = request.StoreId.Value };
          _unitOfWork.Stores.Attach(orgcosmetic.Store);
        }

        _unitOfWork.Brands.Attach(orgcosmetic.Brand);
        _unitOfWork.SkinTypes.Attach(orgcosmetic.SkinType);
        _unitOfWork.CosmeticTypes.Attach(orgcosmetic.CosmeticType);

        // Create the cosmetic
        await _unitOfWork.Cosmetics.CreateAsync(orgcosmetic);

        // Handle thumbnail if provided
        if (request.Thumbnail is not null && request.Thumbnail.Length > 0)
        {
          var filePath = _filePathFactory.CreateFilePath(ObjectType.CosmeticThumbnail, orgcosmetic.Id,
            request.Thumbnail.FileName);
          var uploadRequest = new UploadRequest(filePath, request.Thumbnail);
          var url = await _blobService.UploadBlobsAsync([uploadRequest]);
          orgcosmetic.ThumbnailUrl = url.First();
        }

        // Create the CosmeticPrice entity
        var cosmeticPrice = new CosmeticPrice
        {
          CosmeticId = orgcosmetic.Id,
          OriginalPrice = request.Price,
          // Set default values for required fields
          EventId = new Guid("57934496-4d7c-4e21-aaf6-f42c023033ae"), // Or default event ID if applicable
          StartDate = DateTime.UtcNow,
          EndDate = DateTime.UtcNow.AddYears(10) // Set a far future date or appropriate business logic
        };

        // Save the cosmetic price
        await _unitOfWork.CosmeticPrices.CreateAsync(cosmeticPrice);

        // Save all changes in one go
        var isSaved = await _unitOfWork.CompleteAsync();
        if (!isSaved)
        {
          var error = _errorFactory.CreateDatabaseError("Cosmetic");
          return Result<CosmeticResponse>.Failure([error.err], error.statusCode);
        }

        // Map to response and set price
        var output = orgcosmetic.Adapt<CosmeticResponse>();
        output.Price = request.Price; // Set the price in the response

        return Result<CosmeticResponse>.Success(output, StatusCodes.Status201Created);
      }
      catch (Exception ex)
      {
        var error = _errorFactory.CreateDatabaseError($"Cosmetic creation failed: {ex.Message}");
        return Result<CosmeticResponse>.Failure([error.err], error.statusCode);
      }
    }

    // Helper method for applying sorting
    private IQueryable<Cosmetic> ApplySorting(IQueryable<Cosmetic> query, string sortColumn, string sortOrder)
    {
      bool isAscending = string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);

      return sortColumn.ToLower() switch
      {
        "name" => isAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
        "createat" => isAscending ? query.OrderBy(c => c.CreateAt) : query.OrderByDescending(c => c.CreateAt),
        "brand" => isAscending ? query.OrderBy(c => c.Brand.Name) : query.OrderByDescending(c => c.Brand.Name),
        "skintype" => isAscending ? query.OrderBy(c => c.SkinType.Name) : query.OrderByDescending(c => c.SkinType.Name),
        "cosmetictype" => isAscending
          ? query.OrderBy(c => c.CosmeticType.Name)
          : query.OrderByDescending(c => c.CosmeticType.Name),
        // Note: Can't directly sort by Price since it's calculated from another table
        _ => isAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name), // Default sort
      };
    }

    public async Task<Result<PaginatedList<CosmeticResponse>>> GetCosmeticsAsync(GetCosmeticsRequest request)
    {
      try
      {
        // Get base queryable - store it in a separate variable before applying includes
        var baseQuery = _unitOfWork.Cosmetics.GetQueryable();

        // Apply filters first (on the simple IQueryable)
        foreach (var strategy in _filterStrategies)
        {
          baseQuery = strategy.ApplyFilter<object>(baseQuery, request);
        }

        // Apply sorting if specified (also on the simple IQueryable)
        if (!string.IsNullOrEmpty(request.SortColumn))
        {
          baseQuery = ApplySorting(baseQuery, request.SortColumn, request.SortOrder);
        }

        // Now apply only the necessary includes
        var query = baseQuery
            .Include(c => c.CosmeticImages)
            .Include(c => c.Feedbacks)
            .Include(c => c.Store); // Eagerly load the Store navigation property.

        // Project to a simplified response DTO with only the needed fields
        var cosmeticResponseQuery = query.Select(c => new CosmeticResponse
        {
          Id = c.Id,
          CreateAt = c.CreateAt,
          CreatedBy = c.CreatedBy,
          LastModified = c.LastModified,
          LastModifiedBy = c.LastModifiedBy,
          IsActive = c.IsActive,
          BrandId = c.BrandId,
          SkinTypeId = c.SkinTypeId,
          CosmeticTypeId = c.CosmeticTypeId,
          Name = c.Name,
          Gender = c.Gender,
          Notice = c.Notice,
          Ingredients = c.Ingredients,
          MainUsage = c.MainUsage,
          Texture = c.Texture,
          Origin = c.Origin,
          Instructions = c.Instructions,
          Weight = c.Weight,
          Length = c.Length,
          Width = c.Width,
          Height = c.Height,
          ThumbnailUrl = c.ThumbnailUrl,
          VolumeUnit = c.VolumeUnit,
          // Explicitly map the Store entity to the StoreResponse DTO.
          Store = c.Store == null ? null : new StoreResponse
          {
            Id = c.Store.Id,
            Name = c.Store.Name,
            Address = c.Store.Address,
            Latitude = c.Store.Latitude,
            Longitude = c.Store.Longitude,
            PhoneNumber = c.Store.PhoneNumber,
            OpeningHours = c.Store.OpeningHours
          },
          CosmeticSubcategories = c.CosmeticSubcategories.Select(cs => new CosmeticSubcategoryResponse
          {
            CosmeticId = cs.CosmeticId,
            SubCategoryId = cs.SubCategoryId
          }).ToList(),
          CosmeticImages = c.CosmeticImages.Select(ci => new CosmeticImageResponse()
          {
            Id = ci.Id,
            ImageUrl = ci.ImageUrl,
            CosmeticId = c.Id,
            CosmeticName = c.Name
          }).ToList(),
          Feedbacks = c.Feedbacks.Select(f => new FeedbackCosmeticResponse
          {
            Id = f.Id,
            Rating = f.Rating,
            Content = f.Content
          }).ToList()
        });

        // Create paginated list
        var cosmetics =
          await PaginatedList<CosmeticResponse>.CreateAsync(cosmeticResponseQuery, request.PageIndex, request.PageSize);

        // Get prices for each cosmetic in the results
        foreach (var cosmetic in cosmetics.Items)
        {
          var entity = await _unitOfWork.Cosmetics.GetByIdAsync(cosmetic.Id);
          if (entity != null)
          {
            cosmetic.Price = await _unitOfWork.Cosmetics.GetCosmeticPrice(entity);
            cosmetic.OriginalPrice = await _unitOfWork.Cosmetics.GetCosmeticOriginalPrice(entity);
          }
        }

        // Calculate quantity and rating
        foreach (var cosmetic in cosmetics.Items)
        {
          // Quantity and Rating are calculated properties in your DTO
          // They will be calculated automatically based on the Batches and Feedbacks collections
        }

        return Result<PaginatedList<CosmeticResponse>>.Success(cosmetics, StatusCodes.Status200OK);
      }
      catch (Exception ex)
      {
        return Result<PaginatedList<CosmeticResponse>>.Failure([CosmeticErrors.CosmeticQueryFailue],
          StatusCodes.Status500InternalServerError);
      }
    }

    public async Task<Result<CosmeticResponse>> GetCosmeticById(Guid id)
    {
      // Updated to include the Store information
      var cosmetic = await _unitOfWork.Cosmetics.GetQueryable()
          .Include(c => c.Store)
          .FirstOrDefaultAsync(c => c.Id == id);

      if (cosmetic != null)
      {
        var cosmeticResponse = cosmetic.Adapt<CosmeticResponse>();

        cosmeticResponse.Price = await _unitOfWork.Cosmetics.GetCosmeticPrice(cosmetic);
        cosmeticResponse.OriginalPrice = await _unitOfWork.Cosmetics.GetCosmeticOriginalPrice(cosmetic);
        var images = await _unitOfWork.CosmeticImages.GetCosmeticImagesByCosmeticId(id);
        var imagesDto = images.Select(image => new CosmeticImageResponse
        {
          ImageUrl = image.ImageUrl,
          Id = image.Id,
          CosmeticId = cosmetic.Id,
          CosmeticName = cosmetic.Name
        }).ToList();

        cosmeticResponse.CosmeticImages = imagesDto;

        return Result<CosmeticResponse>.Success(cosmeticResponse, StatusCodes.Status200OK);
      }
      else
      {
        return Result<CosmeticResponse>.Failure([CosmeticErrors.CosmeticNotFound], StatusCodes.Status404NotFound);
      }
    }

    public async Task<Result<List<CosmeticResponse>>> GetCosmeticsByBrandId(Guid brandId)
    {
      var cosmetics = await _unitOfWork.Cosmetics.GetListByAnyId(e => e.BrandId == brandId);
      if (cosmetics != null)
      {
        var cosmeticsResponse = cosmetics.Adapt<List<CosmeticResponse>>();

        // Set price for each cosmetic
        foreach (var response in cosmeticsResponse)
        {
          var cosmetic = cosmetics.First(c => c.Id == response.Id);
          response.Price = await _unitOfWork.Cosmetics.GetCosmeticPrice(cosmetic);
          response.OriginalPrice = await _unitOfWork.Cosmetics.GetCosmeticOriginalPrice(cosmetic);
        }

        return Result<List<CosmeticResponse>>.Success(cosmeticsResponse, StatusCodes.Status200OK);
      }
      else
      {
        return Result<List<CosmeticResponse>>.Failure([CosmeticErrors.CosmeticNotFound], StatusCodes.Status404NotFound);
      }
    }

    public async Task<Result<List<CosmeticResponse>>> GetCosmeticsByName(string name)
    {
      var cosmetics = await _unitOfWork.Cosmetics.GetListByAnyId(e => e.Name == name);
      if (cosmetics != null)
      {
        var cosmeticsResponse = cosmetics.Adapt<List<CosmeticResponse>>();

        // Set price for each cosmetic
        foreach (var response in cosmeticsResponse)
        {
          var cosmetic = cosmetics.First(c => c.Id == response.Id);
          response.Price = await _unitOfWork.Cosmetics.GetCosmeticPrice(cosmetic);
          response.OriginalPrice = await _unitOfWork.Cosmetics.GetCosmeticOriginalPrice(cosmetic);
        }

        return Result<List<CosmeticResponse>>.Success(cosmeticsResponse, StatusCodes.Status200OK);
      }
      else
      {
        return Result<List<CosmeticResponse>>.Failure([CosmeticErrors.CosmeticNotFound], StatusCodes.Status404NotFound);
      }
    }

    public async Task<Result<List<CosmeticResponse>>> GetCosmeticsBySkinTypeId(Guid skinTypeId)
    {
      var cosmetics = await _unitOfWork.Cosmetics.GetListByAnyId(e => e.SkinTypeId == skinTypeId);
      if (cosmetics != null)
      {
        var cosmeticsResponse = cosmetics.Adapt<List<CosmeticResponse>>();

        // Set price for each cosmetic
        foreach (var response in cosmeticsResponse)
        {
          var cosmetic = cosmetics.First(c => c.Id == response.Id);
          response.Price = await _unitOfWork.Cosmetics.GetCosmeticPrice(cosmetic);
          response.OriginalPrice = await _unitOfWork.Cosmetics.GetCosmeticOriginalPrice(cosmetic);
        }

        return Result<List<CosmeticResponse>>.Success(cosmeticsResponse, StatusCodes.Status200OK);
      }
      else
      {
        return Result<List<CosmeticResponse>>.Failure([CosmeticErrors.CosmeticNotFound], StatusCodes.Status404NotFound);
      }
    }

    public async Task<Result<List<CosmeticResponse>>> GetCosmeticsByTypeId(Guid typeId)
    {
      var cosmetics = await _unitOfWork.Cosmetics.GetListByAnyId(e => e.CosmeticTypeId == typeId);
      if (cosmetics != null)
      {
        var cosmeticsResponse = cosmetics.Adapt<List<CosmeticResponse>>();

        // Set price for each cosmetic
        foreach (var response in cosmeticsResponse)
        {
          var cosmetic = cosmetics.First(c => c.Id == response.Id);
          response.Price = await _unitOfWork.Cosmetics.GetCosmeticPrice(cosmetic);
          response.OriginalPrice = await _unitOfWork.Cosmetics.GetCosmeticOriginalPrice(cosmetic);
        }

        return Result<List<CosmeticResponse>>.Success(cosmeticsResponse, StatusCodes.Status200OK);
      }
      else
      {
        return Result<List<CosmeticResponse>>.Failure([CosmeticErrors.CosmeticNotFound], StatusCodes.Status404NotFound);
      }
    }

    public async Task<Result<CosmeticResponse>> UpdateCosmetic(UpdateCosmetic cosmetic, Guid id)
    {
      var existcosmetic = await _unitOfWork.Cosmetics.GetByIdAsync(id);
      if (existcosmetic == null)
      {
        return Result<CosmeticResponse>.Failure([CosmeticErrors.CosmeticNotFound], StatusCodes.Status404NotFound);
      }
      //TODO: HERE
      // var cosmeticPrice = _unitOfWork.CosmeticPrices.GetByIdAsync()
      // Only update if the new value is NOT null
      // existcosmetic.Price = cosmetic.Price != default ? cosmetic.Price : existcosmetic.Price;

      existcosmetic.MainUsage =
        !string.IsNullOrWhiteSpace(cosmetic.MainUsage) ? cosmetic.MainUsage : existcosmetic.MainUsage;
      existcosmetic.Instructions = !string.IsNullOrWhiteSpace(cosmetic.Instructions)
        ? cosmetic.Instructions
        : existcosmetic.Instructions;
      existcosmetic.LastModified = DateTime.Now;

      _unitOfWork.Cosmetics.Update(existcosmetic);
      var isSaved = await _unitOfWork.CompleteAsync();
      if (!isSaved)
      {
        var error = _errorFactory.CreateDatabaseError("Cosmetic");
        return Result<CosmeticResponse>.Failure([error.err], error.statusCode);
      }

      var output = existcosmetic.Adapt<CosmeticResponse>();

      output.Price = await _unitOfWork.Cosmetics.GetCosmeticPrice(existcosmetic);

      return Result<CosmeticResponse>.Success(output, StatusCodes.Status200OK);
    }

    public async Task<Result<CosmeticResponse>> DeleteCosmetic(Guid id)
    {
      var existcosmetic = await _unitOfWork.Cosmetics.GetByIdAsync(id);
      if (existcosmetic == null)
      {
        return Result<CosmeticResponse>.Failure([CosmeticErrors.CosmeticNotFound], StatusCodes.Status404NotFound);
      }

      _unitOfWork.Cosmetics.Remove(existcosmetic);
      var isSaved = await _unitOfWork.CompleteAsync();
      if (!isSaved)
      {
        var error = _errorFactory.CreateDatabaseError("Cosmetic");
        return Result<CosmeticResponse>.Failure([error.err], error.statusCode);
      }

      var output = existcosmetic.Adapt<CosmeticResponse>();

      output.Price = await _unitOfWork.Cosmetics.GetCosmeticPrice(existcosmetic);

      return Result<CosmeticResponse>.Success(output, StatusCodes.Status200OK);
    }

    public async Task<Result<List<CosmeticResponse>>> SearchCosmetics(FilterCosmeticRequest filter)
    {
      var query = await _unitOfWork.Cosmetics.GetAllAsync();

      if (query == null)
      {
        return Result<List<CosmeticResponse>>.Failure(
          [CosmeticErrors.CosmeticNotFound],
          StatusCodes.Status404NotFound
        );
      }

      // Apply filters if they exist
      var filteredResults = query.Where(c =>
        // Name filter (case-insensitive contains)
        (string.IsNullOrEmpty(filter.Name) || c.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)) &&
        // Type filter
        (!filter.TypeId.HasValue || c.CosmeticTypeId == filter.TypeId) &&
        // Brand filter
        (!filter.BrandId.HasValue || c.BrandId == filter.BrandId) &&
        // Skin type filter
        (!filter.SkinTypeId.HasValue || c.SkinTypeId == filter.SkinTypeId)
      ).ToList();

      if (filteredResults?.Any() != true)
      {
        return Result<List<CosmeticResponse>>.Failure(
          [CosmeticErrors.CosmeticNotFound],
          StatusCodes.Status404NotFound
        );
      }

      var cosmeticsResponse = filteredResults.Adapt<List<CosmeticResponse>>();

      foreach (var response in cosmeticsResponse)
      {
        var cosmetic = filteredResults.First(c => c.Id == response.Id);
        response.Price = await _unitOfWork.Cosmetics.GetCosmeticPrice(cosmetic);
        response.OriginalPrice = await _unitOfWork.Cosmetics.GetCosmeticPrice(cosmetic);
      }

      return Result<List<CosmeticResponse>>.Success(
        cosmeticsResponse,
        StatusCodes.Status200OK
      );
    }

    public async Task<Result<CosmeticResponse>> UploadCosmeticImages(CosmeticImagesUploadRequest request,
      string? imageType)
    {
      var validationResult = await _cosmeticImagesUploadValidator.ValidateAsync(request);
      if (!validationResult.IsValid)
      {
        var errors = _errorFactory.CreateValidationError("Images", validationResult);
        return Result<CosmeticResponse>.Failure(errors.errs, errors.statusCode);
      }

      if (!string.IsNullOrEmpty(imageType) &&
          !imageType.Equals(ImageType.THUMBNAIL, StringComparison.OrdinalIgnoreCase))
        return Result<CosmeticResponse>.Failure(
          [new Error("CosmeticImage.InvalidImageType", "Invalid image type for cosmetic")],
          StatusCodes.Status400BadRequest);

      var cosmetic = await _unitOfWork.Cosmetics.GetByIdAsync(request.CosmeticId);

      if (cosmetic is null)
      {
        var error = _errorFactory.CreateNotFoundError("Cosmetic");
        return Result<CosmeticResponse>.Failure([error.err], error.statusCode);
      }

      var uploadRequests = new List<UploadRequest>();
      foreach (var image in request.Images)
      {
        string filePath = string.Empty;
        switch (imageType.ToLower())
        {
          case ImageType.THUMBNAIL:
            filePath = _filePathFactory.CreateFilePath(ObjectType.CosmeticThumbnail, request.CosmeticId,
              image.FileName);
            break;

          default:
            filePath = _filePathFactory.CreateFilePath(ObjectType.CosmeticImage, request.CosmeticId, image.FileName);
            break;
        }

        uploadRequests.Add(new UploadRequest(filePath, image));
      }

      var uploadedUrls = await _blobService.UploadBlobsAsync(uploadRequests);
      if (uploadedUrls == null || !uploadedUrls.Any())
      {
        var errors = _errorFactory.CreateFileCreatedFailed(nameof(uploadedUrls));
        return Result<CosmeticResponse>.Failure([errors.err], errors.statusCode);
      }

      var cosmeticImages = uploadedUrls.Select(url => new CosmeticImage
      {
        CosmeticId = request.CosmeticId,
        ImageUrl = url.ToString()
      }).ToList();

      foreach (var cosmeticImage in cosmeticImages)
      {
        if (imageType.Equals(ImageType.THUMBNAIL, StringComparison.OrdinalIgnoreCase))
        {
          // If image is thumbnail, there is only 1 image file, so we break after update
          var currentThumbnailUrl = cosmetic.ThumbnailUrl;
          if (!string.IsNullOrEmpty(currentThumbnailUrl))
          {
            var isDeleted = await _blobService.DeleteBlobAsync(currentThumbnailUrl);
            if (!isDeleted)
              return Result<CosmeticResponse>.Failure([new Error("CosmeticThumbnail.DeletedFailed", "Thumbnail deleted failed.")], StatusCodes.Status500InternalServerError);
          }

          cosmetic.ThumbnailUrl = cosmeticImage.ImageUrl;
          break;
        }

        await _unitOfWork.CosmeticImages.CreateAsync(cosmeticImage);
      }

      var isSaved = await _unitOfWork.CompleteAsync();
      if (!isSaved)
      {
        var error = _errorFactory.CreateDatabaseError("Cosmetic Images");
        return Result<CosmeticResponse>.Failure([error.err], error.statusCode);
      }

      var response = cosmetic.Adapt<CosmeticResponse>();

      return Result<CosmeticResponse>.Success(response, StatusCodes.Status201Created);
    }
  }
}