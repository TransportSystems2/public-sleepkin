using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Pillow.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        private readonly List<RefreshToken> _refreshTokens = new ();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
        
        public DateTime Created { get; set; }
        
        public bool HasValidRefreshToken(string refreshToken)
        {
            return _refreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public void AddRefreshToken(string token,
            string remoteIpAddress,
            double daysToExpire,
            string deviceName)
        {
            _refreshTokens.Add(
                new RefreshToken(token,
                    DateTime.UtcNow.AddDays(daysToExpire),
                    Id,
                    remoteIpAddress,
                    deviceName));
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            _refreshTokens.Remove(_refreshTokens.First(t => t.Token == refreshToken));
        }
    }
}
