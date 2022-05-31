using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Pillow.PublicApi.AuthEndpoints
{
    public class AuthenticateRequest : BaseRequest 
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(50)]
        public string DeviceName { get; set; }
        
        [CanBeNull] public string TimeZone { get; set; }
    }
}
