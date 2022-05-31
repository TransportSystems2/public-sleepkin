using System;

namespace Pillow.PublicApi.BookEndpoints
{
    public class CreateBookResponse : BaseResponse
    {
        public CreateBookResponse(Guid correlationId) : base(correlationId)
        {
        }

        public CreateBookResponse()
        {
        }

        public BookDto Book { get; set; }
    }
}