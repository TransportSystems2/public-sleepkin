using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Pillow.PublicApi.UsersEndpoints.FavoriteBooks
{
    public class ListPagedFavoriteBookRequest : BaseRequest 
    {
        [FromRoute]
        [Required]
        [MaxLength(36)]
        public string UserName { get; set; }
        
        public int Limit { get; set; } = 20;
        
        public int Offset { get; set; } = 0;

        public string[] Tags { get; set; }
    }
}