using System;

namespace Pillow.PublicApi.UsersEndpoints.FavoriteBooks
{
    public class AddFavoriteBookResponse : BaseResponse
    {
        public AddFavoriteBookResponse()
        {
        }

        public AddFavoriteBookResponse(Guid correlationId) : base(correlationId)
        {
        }
    }
}