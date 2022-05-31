namespace Pillow.ApplicationCore.Interfaces
{
    public interface IRefreshTokenFactory
    {
        string GenerateToken(int size = 32);
    }
}