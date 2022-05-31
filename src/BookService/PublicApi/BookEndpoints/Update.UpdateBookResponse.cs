using System;

namespace Pillow.PublicApi.BookEndpoints
{
    public class UpdateBookResponse : BaseResponse
    {
        public UpdateBookResponse(Guid correlationId)
            : base(correlationId)
        {
        }

        public UpdateBookResponse()
        {
        }
    }
}