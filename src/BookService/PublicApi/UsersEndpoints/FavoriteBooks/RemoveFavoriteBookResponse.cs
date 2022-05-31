using System;

namespace Pillow.PublicApi.UsersEndpoints.FavoriteBooks
{
    public class RemoveFavoriteBookResponse : BaseResponse
    {
        public RemoveFavoriteBookResponse()
        {
        }

        public RemoveFavoriteBookResponse(Guid correlationId) : base(correlationId)
        {
        }
    }
}