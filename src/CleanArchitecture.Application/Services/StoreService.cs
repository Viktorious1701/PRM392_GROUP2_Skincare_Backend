using CleanArchitecture.Application.DTOs.Store;
using CleanArchitecture.Application.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Services
{
  public class StoreService : IStoreService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorFactory _errorFactory;

    public StoreService(IUnitOfWork unitOfWork, IErrorFactory errorFactory)
    {
      _unitOfWork = unitOfWork;
      _errorFactory = errorFactory;
    }

    public async Task<Result<StoreResponse>> CreateStoreAsync(CreateStoreRequest request)
    {
      var store = request.Adapt<Store>();
      _unitOfWork.Stores.Create(store);

      var isSaved = await _unitOfWork.CompleteAsync();
      if (!isSaved)
      {
        var error = _errorFactory.CreateDatabaseError("Store");
        return Result<StoreResponse>.Failure([error.err], error.statusCode);
      }

      var response = store.Adapt<StoreResponse>();
      return Result<StoreResponse>.Success(response, StatusCodes.Status201Created);
    }

    public async Task<Result<List<StoreResponse>>> GetAllStoresAsync()
    {
      var stores = await _unitOfWork.Stores.GetAllAsync();
      var response = stores.Adapt<List<StoreResponse>>();
      return Result<List<StoreResponse>>.Success(response, StatusCodes.Status200OK);
    }

    public async Task<Result<StoreResponse>> GetStoreByIdAsync(Guid id)
    {
      var store = await _unitOfWork.Stores.GetByIdAsync(id);
      if (store == null)
      {
        var error = _errorFactory.CreateNotFoundError("Store");
        return Result<StoreResponse>.Failure([error.err], error.statusCode);
      }

      var response = store.Adapt<StoreResponse>();
      return Result<StoreResponse>.Success(response, StatusCodes.Status200OK);
    }
  }
}