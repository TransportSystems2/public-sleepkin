using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Pillow.PublicApi.UsersEndpoints.FavoriteBooks
{
    public class AddFavoriteBookRequest : BaseRequest
    {
        [Required]
        public string BookCode { get; set; }

        [Required]
        [FromRoute]
        public string UserName { get; set;}
    }
}