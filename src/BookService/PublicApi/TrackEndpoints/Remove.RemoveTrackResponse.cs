using System;

namespace Pillow.PublicApi.TrackEndpoints
{
    public class RemoveTrackResponse : BaseResponse
    {
        public RemoveTrackResponse()
        {
        }

        public RemoveTrackResponse(Guid correlationId) : base(correlationId)
        {
        }

        public string Status { get; set; } = "Removed";
    }
}