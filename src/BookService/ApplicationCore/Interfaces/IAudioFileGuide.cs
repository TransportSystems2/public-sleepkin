using System;

namespace Pillow.ApplicationCore.Interfaces
{
    public interface IAudioFileGuide
    {
        IAudioFile TryToCreate(string path);
    }
    
    public interface IAudioFile
    {
        TimeSpan Duration { get; }
    }
}