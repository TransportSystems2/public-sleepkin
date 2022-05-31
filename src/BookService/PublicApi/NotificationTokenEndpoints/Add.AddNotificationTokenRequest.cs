using System.ComponentModel.DataAnnotations;
using Pillow.ApplicationCore.Entities.NotificationTokenAggregate;

namespace Pillow.PublicApi.NotificationTokenEndpoints
{
    public class AddNotificationTokenRequest : BaseRequest
    {
        [MaxLength(36)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(178)]
        public string Token { get; set; }

        [MaxLength(30)]
        public string DeviceName { get; set; }

        [Required]
        public PlatformTokenKind PlatformKind { get; set; }
    }
}