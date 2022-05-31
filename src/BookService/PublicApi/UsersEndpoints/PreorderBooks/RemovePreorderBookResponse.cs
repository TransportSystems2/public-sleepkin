using System;

namespace Pillow.PublicApi.UsersEndpoints.PreorderBooks
{
    public class RemovePreorderBookResponse : BaseResponse
    {
        public RemovePreorderBookResponse()
        {
        }

        public RemovePreorderBookResponse(Guid correlationId) : base(correlationId)
        {
        }
    }
}