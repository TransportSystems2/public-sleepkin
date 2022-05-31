using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ITelegramBot
    {
        /// <summary>
        /// Отправить сообщение в канал поддержки.
        /// </summary>
        Task SendMessageToSupport(string text);
    }
}