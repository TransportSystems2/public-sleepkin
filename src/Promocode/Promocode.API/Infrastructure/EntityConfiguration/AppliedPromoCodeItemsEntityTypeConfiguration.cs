using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCode.API.Model;

namespace PromoCode.API.Infrastructure.EntityConfiguration
{
    public class AppliedPromoCodeItemsEntityTypeConfiguration : IEntityTypeConfiguration<AppliedPromoCodeItem>
    {
        public void Configure(EntityTypeBuilder<AppliedPromoCodeItem> builder)
        {
            builder.ToTable("AppliedPromoCodes");
            
            builder
                .HasKey(ap => new { Code = ap.PromoCode, ap.UserName });

            builder
                .HasOne(ap => ap.PromoCodeItem)
                .WithMany(p => p.AppliedItems)
                .HasForeignKey(pa => pa.PromoCode);
        }
    }
}