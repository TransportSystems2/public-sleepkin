using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Jose;

namespace Pillow.Infrastructure.Services
{
    public class AppleAccessTokenFactory : IAppleAccessTokenFactory
    {
        public Task<string> CreateToken(string keyId, string issuerKey, string privateKey)
        {
            using var key = ECDsa.Create();
            key.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);

            var header = new Dictionary<string, object>()
            {
                { "kid", keyId }
            };

            var payload = new Dictionary<string, object>()
            {
                { "iss", issuerKey },
                { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };

            return Task.FromResult(Jose.JWT.Encode(payload, key, JwsAlgorithm.ES256, header));
        }
    }
}