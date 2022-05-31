using System;

namespace Pillow.Infrastructure.Identity
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; }

        public DateTime Expires { get; private set; }

        public bool Active => DateTime.UtcNow <= Expires;

        public string RemoteIpAddress { get; private set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; private set; }

        public string DeviceName { get; private set; }

        public RefreshToken(string token,
            DateTime expires,
            string applicationUserId,
            string remoteIpAddress,
            string deviceName)
        {
            Created = DateTime.UtcNow;
            Token = token;
            Expires = expires;
            ApplicationUserId = applicationUserId;
            RemoteIpAddress = remoteIpAddress;
            DeviceName = deviceName;
        }
    }
}