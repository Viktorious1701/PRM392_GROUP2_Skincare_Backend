// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Presentation/Endpoints/StoreController.cs
using CleanArchitecture.Application.DTOs.Store;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Endpoints
{
  public class StoreController : ICarterModule
  {
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      var group = app.MapGroup("api/stores").WithTags("Store Management");

      group.MapGet("/", async (IStoreService storeService) =>
      {
        var result = await storeService.GetAllStoresAsync();
        return result.Match("Retrieved all stores successfully.");
      })
      .WithName("GetAllStores")
      .Produces<ApiResponse<List<StoreResponse>>>(StatusCodes.Status200OK);

      group.MapGet("/{id:guid}", async (IStoreService storeService, Guid id) =>
      {
        var result = await storeService.GetStoreByIdAsync(id);
        return result.Match("Retrieved store successfully.");
      })
      .WithName("GetStoreById")
      .Produces<ApiResponse<StoreResponse>>(StatusCodes.Status200OK)
      .ProducesProblem(StatusCodes.Status404NotFound);

      group.MapPost("/", async (IStoreService storeService, [FromBody] CreateStoreRequest request) =>
      {
        var result = await storeService.CreateStoreAsync(request);
        return result.Match("Store created successfully.");
      })
      .WithName("CreateStore")
      .Produces<ApiResponse<StoreResponse>>(StatusCodes.Status201Created)
      .ProducesProblem(StatusCodes.Status400BadRequest);
    }
  }
}