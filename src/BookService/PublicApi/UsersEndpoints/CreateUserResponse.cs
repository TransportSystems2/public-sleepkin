using System;

namespace Pillow.PublicApi.UsersEndpoints
{
    public class CreateUserResponse : BaseResponse
    {
        public CreateUserResponse(Guid correlationId) : base(correlationId)
        {
        }

        public CreateUserResponse()
        {
        }

        public bool Result { get; set; }
    }
}