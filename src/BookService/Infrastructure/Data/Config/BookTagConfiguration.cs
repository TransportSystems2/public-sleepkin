using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pillow.ApplicationCore.Entities.BookAggregate;

namespace Pillow.Infrastructure.Data.Config
{
    public class BookTagConfiguration : IEntityTypeConfiguration<BookTag>
    {
        public void Configure(EntityTypeBuilder<BookTag> builder)
        {
            builder
                .HasKey(bt => new { bt.BookCode, bt.TagCode });

            builder
                .HasOne(bt => bt.Book)
                .WithMany(b => b.BookTag)
                .HasForeignKey(bt => bt.BookCode);

            builder
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BookTag)
                .HasForeignKey(bt => bt.TagCode);
        }
    }
}