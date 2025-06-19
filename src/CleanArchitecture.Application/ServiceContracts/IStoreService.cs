// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/ServiceContracts/IStoreService.cs
using CleanArchitecture.Application.DTOs.Store;

namespace CleanArchitecture.Application.ServiceContracts
{
  public interface IStoreService
  {
    Task<Result<List<StoreResponse>>> GetAllStoresAsync();
    Task<Result<StoreResponse>> GetStoreByIdAsync(Guid id);
    Task<Result<StoreResponse>> CreateStoreAsync(CreateStoreRequest request);
  }
}