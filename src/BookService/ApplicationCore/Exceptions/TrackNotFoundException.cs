using System;

namespace Pillow.ApplicationCore.Exceptions
{
    public class TrackNotFoundException : ApplicationException
    {
        public TrackNotFoundException(int trackId)
            : base($"No track found with id {trackId}")
        {
        }

        protected TrackNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        public TrackNotFoundException(string message)
            : base(message)
        {
        }

        public TrackNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}