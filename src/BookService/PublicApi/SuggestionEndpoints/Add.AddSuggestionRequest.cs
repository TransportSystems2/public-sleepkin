namespace Pillow.PublicApi.SuggestionEndpoints
{
    public class AddSuggestionRequest : BaseRequest
    {
        public string Text { get; set; }

        public string UserEmail { get; set; }
    }
}