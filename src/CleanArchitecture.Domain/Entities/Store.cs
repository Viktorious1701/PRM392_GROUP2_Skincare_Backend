// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Domain/Entities/Store.cs
namespace CleanArchitecture.Domain.Entities
{
  public class Store : Entity<Guid>
  {
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PhoneNumber { get; set; }
    public string? OpeningHours { get; set; }

    public List<Cosmetic> Cosmetics { get; set; } = new();
  }
}