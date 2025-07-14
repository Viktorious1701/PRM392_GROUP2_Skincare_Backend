using CleanArchitecture.Application.DTOs.Cosmetic;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Endpoints;

public class CosmeticController : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("api/cosmetics").WithTags("Cosmetics Management");

    #region Get Cosmetics API
    group.MapGet("/",
   async (ICosmeticService service, string? name, Guid? brandId, Guid? skinTypeId, Guid? cosmeticTypeId,
         bool? gender, string? sortColumn, string? sortOrder = "asc",
         decimal? minPrice = null, decimal? maxPrice = null, int pageIndex = 1, int pageSize = 10) =>
   {
     var request = new GetCosmeticsRequest(name, brandId, skinTypeId, cosmeticTypeId,
                                          gender, sortColumn, sortOrder,
                                          minPrice, maxPrice, pageIndex, pageSize);
     var result = await service.GetCosmeticsAsync(request);
     return result.Match(Message.SUCCESSFUL_RETRIEVED("Cosmetics"));
   })
 .WithName("GetCosmetics")
 .Produces<ApiResponse<PaginatedList<CosmeticResponse>>>()
 .ProducesProblem(StatusCodes.Status500InternalServerError)
 .WithSummary("GetCosmetics")
 .WithDescription("Get Cosmetics with pagination and filtering options");
    #endregion

    #region Get Cosmetic By Id API
    group.MapGet("/{id}", async (ICosmeticService service, Guid id) =>
    {
      var result = await service.GetCosmeticById(id);
      return result.Match(Message.SUCCESSFUL_RETRIEVED("Cosmetic"));
    })
    .WithName("GetCosmeticById")
    .Produces<ApiResponse<CosmeticResponse>>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithSummary("Get Cosmetic By Id")
    .WithDescription("Retrieve a specific cosmetic by its unique identifier");
    #endregion

    #region Update Cosmetic API
    group.MapPut("/{id}", async (Guid id, ICosmeticService service, UpdateCosmetic updateRequest) =>
    {
      var result = await service.UpdateCosmetic(updateRequest, id);
      return result.Match(Message.SUCCESSFUL_UPDATED("Cosmetic"));
    })
    .WithName("UpdateCosmetic")
    .Produces<ApiResponse<CosmeticResponse>>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithSummary("Update Cosmetic")
    .WithDescription("Update an existing cosmetic's details");
    #endregion

    #region Delete Cosmetic API
    group.MapDelete("/{id}", async (ICosmeticService service, Guid id) =>
    {
      var result = await service.DeleteCosmetic(id);
      return result.Match(Message.SUCCESSFUL_DELETED("Cosmetic"));
    })
    .WithName("DeleteCosmetic")
    .Produces<ApiResponse<CosmeticResponse>>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithSummary("Delete Cosmetic")
    .WithDescription("Delete a cosmetic by its unique identifier");
    #endregion

    #region Create Cosmetic API
    group.MapPost("/", async (ICosmeticService service, [FromForm] CreateCosmetic cosmetic) =>
    {
      var result = await service.CreateCosmetic(cosmetic);
      return result.Match(Message.SUCCESSFUL_CREATED("Cosmetic"));
    })
    .WithName("CreateCosmetic")
    .Produces<ApiResponse<CosmeticResponse>>(StatusCodes.Status201Created)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithSummary("Create Cosmetic")
    .WithDescription("Create a new cosmetic product with all its details")
    .DisableAntiforgery();
    #endregion

    #region Filter Cosmetics API
    group.MapGet("/filter", async (ICosmeticService service, [AsParameters] FilterCosmeticRequest request) =>
    {
      var result = await service.SearchCosmetics(request);
      return result.Match(Message.SUCCESSFUL_RETRIEVED("Cosmetics"));
    })
    .WithName("FilterCosmetics")
    .Produces<ApiResponse<List<CosmeticResponse>>>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithSummary("Filter Cosmetics")
    .WithDescription("Search and filter cosmetics by various criteria");
    #endregion

    #region Upload Cosmetic Thumbnail API

    // group.MapPost("/{cosmeticId}/images/thumbnail", async (ICosmeticImageService service, IFormFile) =>
    // {
    //   
    // });

    #endregion

    #region Upload Cosmetic Images API
    // FIX: Changed the endpoint to directly accept IFormFileCollection.
    // This is the most reliable way to handle file uploads with ASP.NET Core Minimal APIs.
    // The model binder will now correctly populate the 'images' parameter with the uploaded files.
    group.MapPost("/{id}/images", async (ICosmeticService service, Guid id, IFormFileCollection images, [FromQuery] string? imageType = "") =>
    {
      // REASON: We manually construct the request object here. This avoids the model binding confusion
      // that was causing the 'Images' property to be null.
      var request = new CosmeticImagesUploadRequest
      {
        CosmeticId = id,
        Images = images.ToList()
      };

      var result = await service.UploadCosmeticImages(request, imageType);
      return result.Match(Message.SUCCESSFUL_CREATED("Cosmetic Images"));
    })
    .WithName("UploadCosmeticImages")
    .Produces<ApiResponse<CosmeticResponse>>(StatusCodes.Status201Created)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithSummary("Upload Cosmetic Images")
    .WithDescription("Upload multiple images for a specific cosmetic product")
    .DisableAntiforgery();
    #endregion
  }
}