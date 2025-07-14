using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.DTOs.Cosmetic;

public class CosmeticImagesUploadRequest
{
  public Guid CosmeticId { get; set; }
  public List<IFormFile>? Images { get; set; }
}