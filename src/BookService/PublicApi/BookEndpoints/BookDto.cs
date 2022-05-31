using System.Collections.Generic;
using Pillow.ApplicationCore.Enums;
using Pillow.PublicApi.TagEndpoints;
using Pillow.PublicApi.TrackEndpoints;

namespace Pillow.PublicApi.BookEndpoints
{
    public class BookDto
    {
        public string Code { get; set; }

        public TrackDto Trailer { get; set; }

        public List<TrackDto> Tracks { get; set; }

        public List<TagDto> Tags { get; set; }

        public string Title { get; set; }

        public string CoverUri { get; set; }

        public AccessLevel AccessLevel { get; set; }

        public string Author { get; set; }

        public bool IsPublished { get; set; }

        public uint Likes { get; set; }

        public uint Preorders { get; set; }
    }
}