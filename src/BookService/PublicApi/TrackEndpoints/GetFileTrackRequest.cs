namespace Pillow.PublicApi.TrackEndpoints
{
    public class GetFileTrackRequest : BaseRequest 
    {
        public string BookCode { get; set; }
        
        public string TrackCode { get; set; }

        public string TrackFormat {get; set; }
    }
}