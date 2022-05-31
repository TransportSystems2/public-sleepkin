using System;

namespace Pillow.ApplicationCore.Entities.BookAggregate
{
    public class Track : BaseCodeEntity
    {
        public string BookCode { get; set; }

        public virtual Book Book { get; set; }

        public string Name { get; set; }

        public string Format { get; set; }

        public long Size { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsTrailer { get; set; }

        public bool IsRemoved { get; set; }
        
        public bool HasBackgroundMusic { get; set; }
        
        public string Title { get; set; }

        public Track(string code,
            string bookCode,
            string name,
            string title,
            string format,
            long size,
            TimeSpan duration,
            bool isTrailer = false,
            bool hasBackgroundMusic = false)
        {
            Code = code;
            BookCode = bookCode;
            Name = name;
            Title = title;
            Format = format;
            Size = size;
            Duration = duration;
            IsTrailer = isTrailer;
            HasBackgroundMusic = hasBackgroundMusic;
        }
    }
}