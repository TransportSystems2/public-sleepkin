using System;
using System.Collections.Generic;

namespace Pillow.PublicApi.BookEndpoints
{
    public class ListPagedBookResponse : BaseResponse
    {
        public ListPagedBookResponse(Guid correlationId)
            : base(correlationId)
        {
        }

        public ListPagedBookResponse()
        {
        }

        public List<BookDto> Items { get; set; } = new List<BookDto>();
        
        public int Total { get; set; }
    }
}
