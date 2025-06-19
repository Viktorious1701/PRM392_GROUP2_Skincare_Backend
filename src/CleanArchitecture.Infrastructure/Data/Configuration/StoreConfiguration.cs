using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configuration
{
  public class StoreConfiguration : IEntityTypeConfiguration<Store>
  {
    public void Configure(EntityTypeBuilder<Store> builder)
    {
      builder.HasKey(s => s.Id);

      builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
      builder.Property(s => s.Address).IsRequired().HasMaxLength(255);
      builder.Property(s => s.Latitude).IsRequired();
      builder.Property(s => s.Longitude).IsRequired();

      // Configure the one-to-many relationship with Cosmetic
      builder.HasMany(s => s.Cosmetics)
             .WithOne(c => c.Store)
             .HasForeignKey(c => c.StoreId)
             .IsRequired(false); // A cosmetic doesn't have to belong to a store
    }
  }
}