// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/DTOs/Cosmetic/CreateCosmetic.cs
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs.Cosmetic
{
  public class CreateCosmetic
  {
    [Required]
    public Guid BrandId { get; init; }
    [Required]
    public Guid SkinTypeId { get; init; }
    [Required]
    public Guid CosmeticTypeId { get; init; }
    public Guid? StoreId { get; init; } // Add StoreId
    [Required]
    public string Name { get; init; } = default!;
    [Range(0, double.MaxValue)]
    [Required]
    public decimal Price { get; init; }
    public bool Gender { get; init; }
    public string Notice { get; init; } = default!;
    [Required]
    public string Ingredients { get; init; } = default!;
    [Required]
    public string MainUsage { get; init; } = default!;
    public string Texture { get; init; } = default!;
    [Required]
    public string Origin { get; init; } = default!;
    [Required]
    public string Instructions { get; init; } = default!;
    [Range(0, ushort.MaxValue)]
    [Required]
    public ushort Size { get; init; }
    [Required]
    public VolumeUnit VolumeUnit { get; set; }
    public IFormFile? Thumbnail { get; init; } = default!;
  }
}