using System;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.ApplicationCore.Entities.NotificationTokenAggregate
{
    public class NotificationToken : BaseEntity, IAggregateRoot
    {
        public string Token { get; set; }

        public PlatformTokenKind PlatformKind { get; set; }

        public string DeviceName { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedDate { get; private set; }

        public NotificationToken(string token,
            PlatformTokenKind platformKind,
            string deviceName = null,
            string userId = null)
        {
            Token = token;
            PlatformKind = platformKind;
            DeviceName = deviceName;
            UserId = userId;
            CreatedDate = DateTime.UtcNow;
        }
    }
}