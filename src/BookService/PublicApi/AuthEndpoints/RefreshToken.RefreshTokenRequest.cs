using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Pillow.PublicApi.AuthEndpoints
{
    public class RefreshTokenRequest : BaseRequest
    {
        [Required]
        public string AccessToken { get; set; }
        
        [Required]
        public string RefreshToken { get; set; }

        [MaxLength(50)]
        public string DeviceName { get; set; }
        
        [CanBeNull] public string TimeZone { get; set; }
    }
}