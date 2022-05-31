namespace Pillow.Infrastructure.Identity
{
    public class JWTTokenOptions
    {
        /// <summary>
        /// Ключ для подписи токена
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Ключ используется для автоматической генерации пароля пользователя
        /// </summary>
        public string PasswordKey { get; set; }

        /// <summary>
        /// Время жизни AccessToken (мин)
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; } = 15;

        /// <summary>
        /// Время жизни RefreshToken (дней)
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; } = 30;
    }
}