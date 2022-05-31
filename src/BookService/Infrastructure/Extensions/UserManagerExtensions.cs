using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Pillow.Infrastructure.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<IdentityResult> AddOrUpdateClaim<TUser>(this UserManager<TUser> userManager,
            TUser user, Claim claim) where TUser : class
        {
            var userClaims = await userManager.GetClaimsAsync(user);

            var oldClaim = userClaims.FirstOrDefault(c => c.Type.Equals(claim.Type));

            var result = oldClaim switch
            {
                { } when oldClaim.Value == claim.Value => IdentityResult.Success,
                { } => await userManager.ReplaceClaimAsync(user, oldClaim, claim),
                null => await userManager.AddClaimAsync(user, claim),
            };

            return result;
        }
    }
}