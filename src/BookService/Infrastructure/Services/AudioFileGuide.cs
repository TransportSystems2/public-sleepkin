using System;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.Infrastructure.Services
{
    public class AudioFileGuide : IAudioFileGuide
    {
        public IAudioFile TryToCreate(string path)
        {
            try
            {
                TagLib.File fileTag = TagLib.File.Create(path);
                return new AudioFile
                {
                    Duration = fileTag.Properties.Duration
                };
            }
            catch
            {
                return null;
            }
        }
    }
    
    public class AudioFile : IAudioFile
    {
        public TimeSpan Duration { get; init; }
    }
}