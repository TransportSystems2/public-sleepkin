using System;

namespace Pillow.PublicApi.TrackEndpoints
{
    public class AddTrackResponse : BaseResponse
    {
        public AddTrackResponse()
        {
        }

        public AddTrackResponse(Guid correlationId) : base(correlationId)
        {
        }

        public TrackDto Track { get; set; }
    }
}