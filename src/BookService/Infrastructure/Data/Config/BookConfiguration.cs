using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pillow.ApplicationCore.Entities.BookAggregate;

namespace Pillow.Infrastructure.Data.Config
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            IMutableNavigation trackNavigation = builder.Metadata.FindNavigation(nameof(Book.Tracks));
            trackNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            IMutableNavigation bookTagNavigation = builder.Metadata.FindNavigation(nameof(Book.BookTag));
            bookTagNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasKey(b => b.Code);
            
            builder.Property(b => b.Title)
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(b => b.CoverUri)
                .IsRequired(false);

            builder.Property(b => b.AccessLevel)
                .IsRequired(true);

            builder.Property(b => b.Author)
                .HasMaxLength(100);
        }
    }
}