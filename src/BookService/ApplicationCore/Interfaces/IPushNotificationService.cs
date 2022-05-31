using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Сервия для отправки уведомлений
    /// </summary>
    public interface IPushNotificationService
    {
        Task<PushNotificationResult> Push(string[] tokens, string payload);
    }

    /// <summary>
    /// Результат отправки уведомлений
    /// </summary>
    public struct PushNotificationResult
    {
        /// <summary>
        /// Всего токенов
        /// </summary>
        public int NumberOfTotal;

        /// <summary>
        /// Успешно отправленных
        /// </summary>
        public int NumberOfSuccess;

        /// <summary>
        /// Отправленных с ошибкой
        /// </summary>
        public int NumberOfErrors;
    }
}