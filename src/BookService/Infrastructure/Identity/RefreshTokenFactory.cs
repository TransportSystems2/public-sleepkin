using System;
using System.Security.Cryptography;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.Infrastructure.Identity
{
   public sealed class RefreshTokenFactory : IRefreshTokenFactory
    {
        public string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}