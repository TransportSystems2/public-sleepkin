using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pillow.ApplicationCore.Entities.UserAccountAggregate;

namespace Pillow.Infrastructure.Data.Config
{
    public class FavoriteBookConfiguration : IEntityTypeConfiguration<FavoriteBook>
    {
        public void Configure(EntityTypeBuilder<FavoriteBook> builder)
        {
            builder.HasKey(fb => fb.Id);

           builder.Property(fb => fb.BookCode)
                .IsRequired(true);

            builder.Property(fb => fb.UserAccountUserName)
                .IsRequired(true);
        }
    }
}