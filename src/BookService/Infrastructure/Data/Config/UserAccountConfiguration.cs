using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pillow.ApplicationCore.Entities.UserAccountAggregate;

namespace Pillow.Infrastructure.Data.Config
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            IMutableNavigation favoritesNavigation = builder.Metadata.FindNavigation(nameof(UserAccount.Favorites));
            favoritesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasKey(ua => ua.UserName);
        }
    }
}