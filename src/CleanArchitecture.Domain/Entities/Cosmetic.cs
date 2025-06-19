// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Domain/Entities/Cosmetic.cs

using DomainEntity = CleanArchitecture.Domain.Entities;
using System.ComponentModel;

namespace CleanArchitecture.Domain.Entities
{
  public class Cosmetic : Entity<Guid>
  {
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; } = default!;
    public Guid SkinTypeId { get; set; }
    public SkinType SkinType { get; set; } = default!;
    public Guid CosmeticTypeId { get; set; }
    public CosmeticType CosmeticType { get; set; } = default!;
    // Add a nullable foreign key for the Store
    public Guid? StoreId { get; set; }
    // Add a navigation property to the Store
    public Store? Store { get; set; }
    public string Name { get; set; } = default!;
    public bool Gender { get; set; } = true;
    public string Notice { get; set; } = default!;
    public string Ingredients { get; set; } = default!;
    public string MainUsage { get; set; } = default!;
    public string Texture { get; set; } = default!;
    public string Origin { get; set; } = default!;
    public string Instructions { get; set; } = default!;
    public string? ThumbnailUrl { get; set; }
    public int Weight { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public VolumeUnit VolumeUnit { get; set; }
    public List<CosmeticSubCategory> CosmeticSubcategories { get; set; } = new List<CosmeticSubCategory>();
    public List<CosmeticImage> CosmeticImages { get; set; } = new List<CosmeticImage>();
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public List<Batch> Batches { get; set; } = new List<Batch>();
    public List<RoutineStep> RoutineSteps { get; set; } = new List<RoutineStep>();
    public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    public List<RefundItem> RefundItems { get; set; } = new List<RefundItem>();
    public List<CosmeticPrice> CosmeticPrices { get; set; } = new();


  }
  public enum VolumeUnit
  {
    // Use DescriptionAttribute for display variations
    /// <summary>
    ///  ml
    /// </summary>
    ml,
    /// <summary>
    ///  gram
    /// </summary>
    g,
    /// <summary>
    ///  peices
    /// </summary>
    pieces
  }
}