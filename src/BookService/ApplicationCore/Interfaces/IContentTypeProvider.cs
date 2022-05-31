namespace Pillow.ApplicationCore.Interfaces
{
    public interface IContentTypeProvider
    {
        bool TryGetContentType(string subpath, out string contenType);
    }
}