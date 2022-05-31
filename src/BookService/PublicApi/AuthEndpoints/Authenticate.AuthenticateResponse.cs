using System;

namespace Pillow.PublicApi.AuthEndpoints
{
    public class AuthenticateResponse : BaseResponse
    {
        public AuthenticateResponse(Guid correlationId) : base(correlationId)
        {
        }

        public AuthenticateResponse()
        {
        }

        public bool Result { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public long ExpiredIn { get; set; }
    }
}
