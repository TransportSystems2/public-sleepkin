using System;
using Pillow.PublicApi;

namespace PublicApi.NotificationTokenEndpoints.PushEndpoint
{
    public class PushResponse : BaseResponse
    {
        public PushResponse(Guid correlationId) : base(correlationId)
        {
        }

        public PushResponse()
        {
        }
        
        public int NumberOfTotal { get; set; }

        public int NumberOfSuccess { get; set; }

        public int NumberOfErrors { get; set; }
    }
}