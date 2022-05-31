using System.Threading.Tasks;

namespace Pillow.ApplicationCore.Interfaces
{
    public interface ITokenClaimsService
    {
        Task<(string accessToken, long expiredIn)> GetTokenAsync(string userName);
    }
}
