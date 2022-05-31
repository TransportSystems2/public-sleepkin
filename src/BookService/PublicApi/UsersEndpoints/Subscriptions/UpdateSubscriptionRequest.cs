using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Pillow.PublicApi;

namespace PublicApi.UsersEndpoints.Subscriptions
{
    public class UpdateSubscriptionRequest : BaseRequest
    {
        [FromRoute]
        [Required]
        public string UserName { get; set; }

        [Required]
        public string ReceiptData { get; set; }

        public bool ExcludeOldTransaction { get; set;}
    }
}