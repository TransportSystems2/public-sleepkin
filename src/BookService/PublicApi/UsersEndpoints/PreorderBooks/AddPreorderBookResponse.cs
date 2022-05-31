using System;

namespace Pillow.PublicApi.UsersEndpoints.PreorderBooks
{
    public class AddPreorderBookResponse : BaseResponse
    {
        public AddPreorderBookResponse()
        {
        }

        public AddPreorderBookResponse(Guid correlationId) : base(correlationId)
        {
        }
    }
}