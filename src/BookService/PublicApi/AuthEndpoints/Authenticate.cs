using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pillow.ApplicationCore.Interfaces;
using Pillow.Infrastructure.Identity;
using Pillow.PublicApi.Utils;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Pillow.ApplicationCore.Constants;
using Pillow.Infrastructure.Extensions;

namespace Pillow.PublicApi.AuthEndpoints
{
    public class Authenticate : BaseAsyncEndpoint<AuthenticateRequest, AuthenticateResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenClaimsService _tokenClaimsService;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly JWTTokenOptions _jwtTokenOptions;
        private readonly ILogger _logger;

        public Authenticate(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenClaimsService tokenClaimsService,
            IRefreshTokenFactory refreshTokenFactory, 
            IOptions<JWTTokenOptions> jwtTokenOptions,
            ILogger<Authenticate> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenClaimsService = tokenClaimsService;
            _refreshTokenFactory = refreshTokenFactory;
            _jwtTokenOptions = jwtTokenOptions.Value ?? throw new ArgumentNullException(nameof(jwtTokenOptions));
            _logger = logger;
        }

        [HttpPost("api/auth")]
        [SwaggerOperation(
            Summary = "Authenticates a user",
            Description = "Authenticates a user",
            OperationId = "auth.auth",
            Tags = new[] { "AuthEndpoints" })
        ]
        public override async Task<ActionResult<AuthenticateResponse>> HandleAsync(AuthenticateRequest request,
            CancellationToken cancellationToken)
        {
            var response = new AuthenticateResponse(request.CorrelationId());

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

            if (!result.Succeeded)
            {
                return Unauthorized(result);
            }

            response.Result = result.Succeeded;
            
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (request.TimeZone != null)
            {
                await _userManager.AddOrUpdateClaim(user,
                    new Claim(ClaimConstans.TimeZone, request.TimeZone));
            }

            var token = await _tokenClaimsService.GetTokenAsync(request.UserName);

            response.AccessToken = token.accessToken;
            string refreshToken = _refreshTokenFactory.GenerateToken();
            user.AddRefreshToken(refreshToken,
                HttpContext.RemoteIpAddress(),
                _jwtTokenOptions.RefreshTokenExpirationDays,
                request.DeviceName);
            
            await _userManager.UpdateAsync(user);

            response.RefreshToken = refreshToken;
            response.ExpiredIn = token.expiredIn;

            return response;
        }
    }
}
