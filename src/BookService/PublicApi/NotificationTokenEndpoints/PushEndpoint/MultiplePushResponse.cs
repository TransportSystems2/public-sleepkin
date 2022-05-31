using System;

namespace Pillow.PublicApi.NotificationTokenEndpoints.PushEndpoint
{
    public class MultiplePushResponse : BaseResponse
    {
        public MultiplePushResponse(Guid correlationId) : base(correlationId)
        {
        }

        public MultiplePushResponse()
        {
        }

        public int NumberOfTotal { get; set; }

        public int NumberOfSuccess { get; set; }

        public int NumberOfErrors { get; set; }
    }
}