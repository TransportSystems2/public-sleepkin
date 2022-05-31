using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Фабрика по созданию токенов доступа для Apple серверов 
    /// </summary>
    public interface IAppleAccessTokenFactory
    {
        Task<string> CreateToken(string keyId, string issuerKey, string privateKey);
    }
}