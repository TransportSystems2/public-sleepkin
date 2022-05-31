using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pillow.ApplicationCore.Entities.SubscriptionAggregate;

namespace Pillow.Infrastructure.Data.Config
{
    public class SubscriptionConfiguration: IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(fb => fb.UserAccountUserName)
                .IsRequired(true);
        }
    }
}