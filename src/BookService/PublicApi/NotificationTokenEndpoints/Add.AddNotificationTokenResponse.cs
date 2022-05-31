
using System;

namespace Pillow.PublicApi.NotificationTokenEndpoints
{
    public class AddNotificationTokenResponse : BaseResponse
    {
        public AddNotificationTokenResponse(Guid correlationId) : base(correlationId)
        {
        }

        public AddNotificationTokenResponse()
        {
        }
    }
}