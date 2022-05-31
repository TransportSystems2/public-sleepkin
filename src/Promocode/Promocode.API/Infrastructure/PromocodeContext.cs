using Microsoft.EntityFrameworkCore;
using PromoCode.API.Infrastructure.EntityConfiguration;
using PromoCode.API.Model;

namespace PromoCode.API.Infrastructure
{
    public class PromoCodeContext : DbContext
    {
        public PromoCodeContext(DbContextOptions<PromoCodeContext> options)
            : base(options)
        {
        }
        
        public DbSet<PromoCodeItem> PromoCodeItems { get; set; }
        
        public DbSet<AppliedPromoCodeItem> AppliedPromoCodeItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCollation("case_insensitive", "und-u-ks-level2", "icu", false);
            
            modelBuilder.ApplyConfiguration(new PromoCodeItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AppliedPromoCodeItemsEntityTypeConfiguration());
        }
    }
}