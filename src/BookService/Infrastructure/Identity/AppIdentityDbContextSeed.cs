using Microsoft.AspNetCore.Identity;
using Pillow.ApplicationCore.Constants;
using System.Threading.Tasks;

namespace Pillow.Infrastructure.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(AuthorizationConstants.Roles.Moderator))
                await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.Moderator));

            string moderatorUserName = "moderator@sleepkin.ru";
            if (await userManager.FindByNameAsync(moderatorUserName) == null)
            {
                var moderatorUser = new ApplicationUser { UserName = moderatorUserName, Email = moderatorUserName };
                await userManager.CreateAsync(moderatorUser, AuthorizationConstants.ModeratorPassword);
                moderatorUser = await userManager.FindByNameAsync(moderatorUserName);
                await userManager.AddToRoleAsync(moderatorUser, AuthorizationConstants.Roles.Moderator);
            }
        }
    }
}
