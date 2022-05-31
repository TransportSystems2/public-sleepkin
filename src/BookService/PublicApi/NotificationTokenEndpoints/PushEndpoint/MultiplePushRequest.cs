using System.ComponentModel.DataAnnotations;

namespace Pillow.PublicApi.NotificationTokenEndpoints.PushEndpoint
{
    public class MultiplePushRequest : BaseRequest
    {
        [Required]
        public string Message { get; set; }
    }
}