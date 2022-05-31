using System;

namespace Pillow.PublicApi.BookEndpoints
{
    public class DeleteBookResponse : BaseResponse
    {
        public DeleteBookResponse()
        {
        }

        public DeleteBookResponse(Guid correlationId) : base(correlationId)
        {
        }

        public string Status { get; set; } = "Deleted";
    }
}