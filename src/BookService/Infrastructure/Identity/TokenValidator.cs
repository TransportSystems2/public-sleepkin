using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.Infrastructure.Identity
{
    public class TokenValidator : ITokenValidator
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenValidator()
        {
            if (_jwtSecurityTokenHandler == null)
                _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateLifetime = false 
            };

            SecurityToken securityToken;
            ClaimsPrincipal principal;
            try
            {
                principal = _jwtSecurityTokenHandler.ValidateToken(token,
                    tokenValidationParameters,
                    out securityToken);
            }
            catch (Exception e)
            {
                throw new SecurityTokenException("Invalid token", e);
            }

            if (!(securityToken is JwtSecurityToken jwtSecurityToken)
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}