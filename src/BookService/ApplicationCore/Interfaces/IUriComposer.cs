namespace Pillow.ApplicationCore.Interfaces
{
    public interface IUriComposer
    {
        string ComposePicUri(string uriTemplate);

        string ComposeTrackUri(string bookCode, string trackCode, string format);

        string ComposeTrackFilePath(string trackCode, string format);

        string ComposeTempTrackFilePath(string trackCode, string format);

        string GetTrackPath(string trackCode, string format);
    }
}