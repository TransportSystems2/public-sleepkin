using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pillow.ApplicationCore.Constants;
using Pillow.ApplicationCore.Interfaces;
using Pillow.Infrastructure.Extensions;
using Pillow.Infrastructure.Identity;
using Pillow.PublicApi.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.AuthEndpoints
{
    public class RefreshToken : BaseAsyncEndpoint<RefreshTokenRequest, RefreshTokenResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenClaimsService _tokenClaimsService;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly ITokenValidator _tokenValidator;
        private readonly JWTTokenOptions _jwtTokenOptions;

        public RefreshToken(UserManager<ApplicationUser> userManager,
            ITokenClaimsService tokenClaimsService,
            IRefreshTokenFactory refreshTokenFactory,
            ITokenValidator tokenValidator,
            IOptions<JWTTokenOptions> jwtTokenOptions)
        {
            _userManager = userManager;
            _tokenClaimsService = tokenClaimsService;
            _refreshTokenFactory = refreshTokenFactory;
            _tokenValidator = tokenValidator;
            _jwtTokenOptions = jwtTokenOptions.Value ?? throw new ArgumentNullException(nameof(jwtTokenOptions));
        }

        [HttpPost("api/auth/refreshtoken")]
        [SwaggerOperation(
            Summary = "Refresh token",
            Description = "Refresh token",
            OperationId = "auth.refreshtoken",
            Tags = new[] { "AuthEndpoints" })
        ]
        public override async Task<ActionResult<RefreshTokenResponse>> HandleAsync(
            RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            var response = new RefreshTokenResponse(request.CorrelationId());
            ClaimsPrincipal claimsPrincipal;
            try
            {
                claimsPrincipal = _tokenValidator.GetPrincipalFromToken(request.AccessToken, _jwtTokenOptions.SecretKey);
            }
            catch (SecurityTokenException)
            {
                claimsPrincipal = null;
            }

            response.Result = claimsPrincipal != null;

            if (claimsPrincipal != null)
            {
                var userNameClaim = claimsPrincipal.Claims.First(c => c.Type == ClaimConstans.Name);
                var userName = userNameClaim.Value;
                var user = await _userManager.Users
                    .Include(x => x.RefreshTokens)
                    .SingleAsync(x => x.UserName == userName);

                if (user.HasValidRefreshToken(request.RefreshToken))
                {
                    if (request.TimeZone != null)
                    {
                        await _userManager.AddOrUpdateClaim(user,
                            new Claim(ClaimConstans.TimeZone, request.TimeZone));
                    }

                    var token = await _tokenClaimsService.GetTokenAsync(userName);
                    var refreshToken = _refreshTokenFactory.GenerateToken();
                    user.RemoveRefreshToken(request.RefreshToken); 
                    user.AddRefreshToken(refreshToken,
                        HttpContext.RemoteIpAddress(),
                        _jwtTokenOptions.RefreshTokenExpirationDays,
                        request.DeviceName);
                    await _userManager.UpdateAsync(user);
                    
                    response.AccessToken = token.accessToken;
                    response.RefreshToken = refreshToken;
                    response.ExpiredIn = token.expiredIn;
                }
                else
                {
                    response.Result = false;
                }
            }

            return response;
        }
    }
}