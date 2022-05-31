using Microsoft.AspNetCore.Mvc;

namespace Pillow.PublicApi.BookEndpoints
{
    public class UpdateBookRequest : BaseRequest
    {
        [FromRoute]
        public string BookCode { get; set; }

        public bool? IsPublished { get; set; }

        public string Title { get; set; }
    }
}