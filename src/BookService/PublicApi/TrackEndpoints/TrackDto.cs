using System;

namespace Pillow.PublicApi.TrackEndpoints
{
    public class TrackDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
        
        public string Title { get; set; }
        
        public string Format { get; set; }

        public string Path { get; set; }

        public int Size { get; set; }
        
        public bool HasBackgroundMusic { get; set; }

        public TimeSpan Duration { get; set; }
    }
}