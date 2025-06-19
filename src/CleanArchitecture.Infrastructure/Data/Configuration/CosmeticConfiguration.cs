using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configuration
{
  public class CosmeticConfiguration : IEntityTypeConfiguration<Cosmetic>
  {
    public void Configure(EntityTypeBuilder<Cosmetic> builder)
    {
      builder.HasKey(cosmetic => cosmetic.Id);

      builder.HasOne(c => c.Brand)
        .WithMany(b => b.Cosmetics)
        .HasForeignKey(c => c.BrandId);

      builder.HasOne(c => c.SkinType)
        .WithMany(st => st.Cosmetics)
        .HasForeignKey(c => c.SkinTypeId);

      builder.HasOne(c => c.CosmeticType)
        .WithMany(ct => ct.Cosmetics)
        .HasForeignKey(c => c.CosmeticTypeId);

      // Add this relationship configuration
      builder.HasOne(c => c.Store)
        .WithMany(s => s.Cosmetics)
        .HasForeignKey(c => c.StoreId)
        .IsRequired(false); // Makes the relationship optional

      builder.HasMany(cosmetic => cosmetic.CosmeticSubcategories)
        .WithOne(orderItem => orderItem.Cosmetic);

      builder.HasMany(cosmetic => cosmetic.CosmeticImages)
        .WithOne(orderItem => orderItem.Cosmetic);

      builder.HasMany(cosmetic => cosmetic.CartItems)
        .WithOne(orderItem => orderItem.Cosmetic);

      builder.HasMany(cosmetic => cosmetic.OrderItems)
        .WithOne(orderItem => orderItem.Cosmetic);

      builder.HasMany(cosmetic => cosmetic.Batches)
        .WithOne(orderItem => orderItem.Cosmetic);

      builder.HasMany(cosmetic => cosmetic.RoutineSteps)
        .WithOne(orderItem => orderItem.Cosmetic);

      builder.HasMany(cosmetic => cosmetic.Feedbacks)
        .WithOne(orderItem => orderItem.Cosmetic);

      builder.HasMany(cosmetic => cosmetic.RefundItems)
        .WithOne(orderItem => orderItem.Cosmetic);

      builder.Property(cosmetic => cosmetic.ThumbnailUrl).IsRequired(false);
    }
  }
}