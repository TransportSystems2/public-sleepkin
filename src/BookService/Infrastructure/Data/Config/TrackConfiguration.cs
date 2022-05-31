using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pillow.ApplicationCore.Entities.BookAggregate;

namespace Pillow.Infrastructure.Data.Config
{
    public class TrackConfiguration : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.HasKey(t => t.Code);

            builder.Property(t => t.Name)
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(t => t.Title)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(t => t.Format)
                .IsRequired(true);

            builder.Property(t => t.IsTrailer)
                .HasDefaultValue(false);
        }
    }
}