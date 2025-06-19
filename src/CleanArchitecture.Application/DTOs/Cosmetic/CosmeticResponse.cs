// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/DTOs/Cosmetic/CosmeticResponse.cs
using CleanArchitecture.Application.DTOs.BatchDto;
using CleanArchitecture.Application.DTOs.BrandDto;
using CleanArchitecture.Application.DTOs.CosmeticImageDto;
using CleanArchitecture.Application.DTOs.CosmeticSubcategory;
using CleanArchitecture.Application.DTOs.CosmeticTypeDto;
using CleanArchitecture.Application.DTOs.FeedbackDto;
using CleanArchitecture.Application.DTOs.SkinTypeDto;
using CleanArchitecture.Application.DTOs.Store;
using System.Text.Json.Serialization;
namespace CleanArchitecture.Application.DTOs.Cosmetic
{
  public class CosmeticResponse
  {
    public Guid Id { get; set; }
    public DateTime? CreateAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsActive { get; set; }
    public Guid BrandId { get; set; }
    public Guid SkinTypeId { get; set; }
    public Guid CosmeticTypeId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public bool Gender { get; set; } = true;
    public string Notice { get; set; } = default!;
    public string Ingredients { get; set; } = default!;
    public string MainUsage { get; set; } = default!;
    public string Texture { get; set; } = default!;
    public string Origin { get; set; } = default!;
    public string Instructions { get; set; } = default!;
    public int Weight { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string? ThumbnailUrl { get; set; }
    public VolumeUnit VolumeUnit { get; set; }
    public StoreResponse? Store { get; set; } // Include store information
    public List<CosmeticSubcategoryResponse>? CosmeticSubcategories { get; set; }
    public List<CosmeticImageResponse>? CosmeticImages { get; set; }
    public List<FeedbackCosmeticResponse>? Feedbacks { get; set; }
    public List<BatchResponse>? Batches { get; set; }
    public int Quantity =>
      Batches?
          .Where(b => b.ExpirationDate > DateOnly.FromDateTime(DateTime.Now))
          .Sum(b => b.Quantity) ?? 0;

    public decimal? Rating =>
        Feedbacks?.Count > 0
            ? Feedbacks.Average(f => f.Rating)
            : 0;
  }
}