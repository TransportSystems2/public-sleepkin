using System.ComponentModel.DataAnnotations;

namespace Pillow.PublicApi.UsersEndpoints
{
    public class CreateUserRequest : BaseRequest
    {
        [Required]
        [MaxLength(36)]
        public string UserName { get; set; }
    }
}