using CleanArchitecture.Application.DTOs.Cosmetic;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Validators.Cosmetic;

public class CosmeticImagesUploadValidator : AbstractValidator<CosmeticImagesUploadRequest>
{
  public CosmeticImagesUploadValidator()
  {
    RuleFor(x => x.CosmeticId)
        .NotEmpty().WithMessage("Cosmetic ID is required.");

    // This is the primary rule that checks if the list exists and is not empty.
    RuleFor(x => x.Images)
        .NotNull().WithMessage("Image list cannot be null.")
        .NotEmpty().WithMessage("At least one image is required.");

    // This rule, which was causing the crash, is now conditionally executed.
    // It will ONLY run if the Images list is not null, preventing the NullReferenceException.
    RuleFor(x => x.Images)
        .Must(images => images.Count <= 10).WithMessage("You can upload up to 10 images.")
        .When(x => x.Images != null); // This is the crucial fix.

    // This rule for validating each file is also made conditional for extra safety.
    RuleForEach(x => x.Images)
        .SetValidator(new ImageFileValidator())
        .When(x => x.Images != null);
  }
}

public class ImageFileValidator : AbstractValidator<IFormFile>
{
  private readonly List<string> _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
  private const int MaxFileSize = 5 * 1024 * 1024; // 5MB

  public ImageFileValidator()
  {
    RuleFor(x => x.Length)
        .GreaterThan(0).WithMessage("File cannot be empty.")
        .LessThanOrEqualTo(MaxFileSize).WithMessage($"File size must not exceed {MaxFileSize / (1024 * 1024)}MB.");

    RuleFor(x => x.FileName)
        .Must(fileName => _allowedExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
        .WithMessage($"Only {string.Join(", ", _allowedExtensions)} file types are allowed.");
  }
}