using System;

namespace Pillow.PublicApi.AuthEndpoints
{
    public class RefreshTokenResponse : BaseResponse
    {
        public RefreshTokenResponse(Guid correlationId) : base(correlationId)
        {
        }

        public RefreshTokenResponse()
        {
        }

        public bool Result { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public long ExpiredIn { get; set; }
    }
}