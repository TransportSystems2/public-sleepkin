using System.Security.Claims;

namespace Pillow.ApplicationCore.Interfaces
{
    public interface ITokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}