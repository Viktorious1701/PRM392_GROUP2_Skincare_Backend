// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Infrastructure/Repositories/StoreRepository.cs
using CleanArchitecture.Domain.RepositoryContracts;

namespace CleanArchitecture.Infrastructure.Repositories
{
  public class StoreRepository : GenericRepository<Store>, IStoreRepository
  {
    public StoreRepository(ApplicationDbContext context) : base(context)
    {
    }
  }
}