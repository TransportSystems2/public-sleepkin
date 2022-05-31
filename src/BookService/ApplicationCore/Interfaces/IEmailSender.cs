using System.Threading.Tasks;

namespace Pillow.ApplicationCore.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
