using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Pillow.PublicApi.TrackEndpoints
{
    public class RemoveTrackRequest : BaseRequest
    {
        [FromQuery]
        public bool IsHardRemove { get; set; }

        [Required]
        public string BookCode { get; set; }

        [Required]
        public string TrackCode { get; set; }
    }
}