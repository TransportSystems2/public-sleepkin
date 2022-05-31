namespace Pillow.ApplicationCore.Enums
{
    /// <summary>
    /// Доступность книги
    /// </summary>
    public enum AccessLevel
    {
        /// <summary>
        /// Бесплатно
        /// </summary>
        Free = 0,
        
        /// <summary>
        /// Можно купить
        /// </summary>
        Paid = 1,

        /// <summary>
        /// По подписке
        /// </summary>
        Subscription = 2
    }
}