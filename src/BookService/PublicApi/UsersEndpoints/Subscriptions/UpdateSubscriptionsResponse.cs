using System;
using Pillow.PublicApi;

namespace PublicApi.UsersEndpoints.Subscriptions
{
    public class UpdateSubscriptionsResponse : BaseResponse
    {
        public UpdateSubscriptionsResponse()
        {
        }

        public UpdateSubscriptionsResponse(Guid correlationId) : base(correlationId)
        {
        }
    }
}