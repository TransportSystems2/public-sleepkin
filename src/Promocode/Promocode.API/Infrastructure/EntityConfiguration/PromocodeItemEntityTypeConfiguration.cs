using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCode.API.Model;

namespace PromoCode.API.Infrastructure.EntityConfiguration
{
    public class PromoCodeItemEntityTypeConfiguration : IEntityTypeConfiguration<PromoCodeItem>
    {
        public void Configure(EntityTypeBuilder<PromoCodeItem> builder)
        {
            builder.ToTable("PromoCode");

            builder.Property(pi => pi.PromoCode)
                .UseCollation("case_insensitive");
            builder.HasKey(pi => pi.PromoCode);
            
            builder.Property(pi => pi.Description)
                .IsRequired(false)
                .HasMaxLength(100);
            
            builder.Property(pi => pi.IsActive)
                .HasDefaultValue(false);

            builder.Property(pi => pi.Limit)
                .HasDefaultValue(-1);

            builder.Property(pi => pi.SubscriptionsGroup)
                .HasMaxLength(100);
            
            IMutableNavigation appliedItemsNavigation = builder.Metadata.FindNavigation(nameof(PromoCodeItem.AppliedItems));
            appliedItemsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}