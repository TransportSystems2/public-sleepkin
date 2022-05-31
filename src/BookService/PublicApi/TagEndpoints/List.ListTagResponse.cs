using System;
using System.Collections.Generic;

namespace Pillow.PublicApi.TagEndpoints
{
    public class ListTagResponse : BaseResponse
    {
        public ListTagResponse(Guid correlationId) : base(correlationId)
        {
        }

        public ListTagResponse()
        {
        }

        public List<TagDto> Items { get; set; } = new List<TagDto>();
    }
}