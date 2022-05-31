using System.ComponentModel.DataAnnotations;

namespace Pillow.PublicApi.UsersEndpoints.PreorderBooks
{
    public class RemovePreorderBookRequest : BaseRequest
    {
        [Required]
        public string UserName { get; set;}

        [Required]
        public string BookCode { get; set; }
    }
}