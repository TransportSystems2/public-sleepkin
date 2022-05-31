using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Entities.TagAggregate;

namespace Pillow.Infrastructure.Data.Config
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            IMutableNavigation bookTagNavigation = builder.Metadata.FindNavigation(nameof(Book.BookTag));
            bookTagNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasKey(t => t.Code);

            builder.Property(t => t.Title)
                .IsRequired(true)
                .HasMaxLength(50);
        }
    }
}