using System.ComponentModel.DataAnnotations;

namespace Pillow.PublicApi.UsersEndpoints.FavoriteBooks
{
    public class RemoveFavoriteBookRequest : BaseRequest
    {
        [Required]
        public string UserName { get; set;}

        [Required]
        public string BookCode { get; set; }
    }
}