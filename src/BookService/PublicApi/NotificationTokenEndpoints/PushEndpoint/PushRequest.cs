using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Pillow.PublicApi;

namespace PublicApi.NotificationTokenEndpoints.PushEndpoint
{
    public class PushRequest : BaseRequest
    {
        [FromRoute]
        [Required]
        public string Token { get; set; }

        [Required]
        public string Message { get; set; }
    }
}