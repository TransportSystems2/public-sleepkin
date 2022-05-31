using Microsoft.AspNetCore.Identity;
using Pillow.ApplicationCore.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pillow.ApplicationCore.Constants;
using Pillow.ApplicationCore.Entities.SubscriptionAggregate;
using Pillow.ApplicationCore.Extensions;
using Pillow.ApplicationCore.Services.Subscriptions.Specifications;

namespace Pillow.Infrastructure.Identity
{
    public class IdentityTokenClaimService : ITokenClaimsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTTokenOptions _jwtTokenOptions;
        private readonly IAsyncRepository<Subscription> _subscriptionRepository;

        public IdentityTokenClaimService(UserManager<ApplicationUser> userManager,
            IAsyncRepository<Subscription> subscriptionRepository,
            IOptions<JWTTokenOptions> jwtTokenOptions)
        {
            _userManager = userManager;
            _subscriptionRepository = subscriptionRepository;
            _jwtTokenOptions = jwtTokenOptions.Value ?? throw new ArgumentNullException(nameof(jwtTokenOptions));
        }

        public async Task<(string accessToken, long expiredIn)> GetTokenAsync(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtTokenOptions.SecretKey);
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, userName),
                new (ClaimConstans.Created, user.Created.ToDateTimeString())
            };
            
            var userClaims = await _userManager.GetClaimsAsync(user);
            var timeZoneClaim = userClaims.FirstOrDefault(c => c.Type.Equals(ClaimConstans.TimeZone));
            if (timeZoneClaim != null)
                claims.Add(timeZoneClaim);
            
            var customClaims = await GetCustomClaims(userName);
            if (customClaims != null)
            {
                claims.AddRange(customClaims);
            }

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expiredIn = DateTime.UtcNow.AddMinutes(_jwtTokenOptions.AccessTokenExpirationMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = expiredIn,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), ((DateTimeOffset)expiredIn).ToUnixTimeSeconds());
        }
        
        private async Task<List<Claim>> GetCustomClaims(string userName)
        {
            var result = new List<Claim>();
            
            var subscriptionSpecification = new SubscriptionsByUserSpecification(userName);
            var subscription = await _subscriptionRepository.FirstOrDefaultAsync(subscriptionSpecification);
            if (subscription != null && subscription.IsActive())
            {
                result.Add(new Claim(ClaimConstans.HasSubscription, "true"));
            }

            return result;
        }
    }
}
