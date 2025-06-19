// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/Mapper/StoreMappingConfig.cs
using CleanArchitecture.Application.DTOs.Store;
using Mapster;

namespace CleanArchitecture.Application.Mapper
{
  public class StoreMappingConfig : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<Store, StoreResponse>();
      config.NewConfig<CreateStoreRequest, Store>();
    }
  }
}