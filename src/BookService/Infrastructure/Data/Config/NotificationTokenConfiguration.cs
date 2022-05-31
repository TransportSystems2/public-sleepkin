using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pillow.ApplicationCore.Entities.NotificationTokenAggregate;

namespace Pillow.Infrastructure.Data.Config
{
    public class NotificationTokenConfiguration : IEntityTypeConfiguration<NotificationToken>
    {
        public void Configure(EntityTypeBuilder<NotificationToken> builder)
        {
            builder.HasKey(t => t.Token);
            
            builder.Property(t => t.Token)
                .HasMaxLength(178);

            builder.Property(t => t.UserId)
                .IsRequired(false)
                .HasMaxLength(36);

            builder.Property(t => t.DeviceName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(t => t.PlatformKind)
                .IsRequired(true);
        }
    }
}