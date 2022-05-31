using Pillow.ApplicationCore.Enums;

namespace Pillow.PublicApi.BookEndpoints
{
    public class CreateBookRequest : BaseRequest
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public string CoverUri { get; set; }

        public AccessLevel AccessLevel { get; set; }

        public string Author { get; set; }
    }
}