using System;
using System.Collections.Generic;

namespace Pillow.PublicApi.UsersEndpoints.FavoriteBooks
{
    public class ListPagedFavoriteBookResponse : BaseResponse
    {
        public ListPagedFavoriteBookResponse(Guid correlationId)
            : base(correlationId)
        {
        }

        public ListPagedFavoriteBookResponse()
        {
        }

        public List<FavoriteBookDto> Items { get; set; } = new List<FavoriteBookDto>();
        
        public int Total { get; set; }
    }
}
