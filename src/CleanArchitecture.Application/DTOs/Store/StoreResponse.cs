// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/DTOs/Store/StoreResponse.cs
namespace CleanArchitecture.Application.DTOs.Store
{
  public class StoreResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PhoneNumber { get; set; }
    public string? OpeningHours { get; set; }
  }
}