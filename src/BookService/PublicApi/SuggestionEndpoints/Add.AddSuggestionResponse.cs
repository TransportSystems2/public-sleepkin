using System;

namespace Pillow.PublicApi.SuggestionEndpoints
{
    public class AddSuggestionResponse : BaseResponse
    {
        public AddSuggestionResponse(Guid correlationId) : base(correlationId)
        {
        }

        public AddSuggestionResponse()
        {
        }
    }
}