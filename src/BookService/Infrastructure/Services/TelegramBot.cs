using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Infrastructure.Services
{
    public class TelegramBot : ITelegramBot
    {
        private ITelegramBotClient _client;
        private TelegramBotOptions _options;
        private ILogger _logger;
        
        public TelegramBot(IOptions<TelegramBotOptions> options, ILogger<TelegramBot> logger)
        {
            _logger = logger;
            
            if (options == null)
                throw new ArgumentNullException("Не заданы настройки для Телеграм бота");
            
            _options = options.Value;
            if (string.IsNullOrEmpty(_options.Token))
                throw new ArgumentNullException("Не задан токен для Телеграм бота");

            _client = new TelegramBotClient(_options.Token);
        }

        public async Task SendMessageToSupport(string text)
        {
            if (_options.SupportChatId == 0)
                throw new ArgumentException("Ну указан id чата поддержки для Телеграм бота");

            await _client.SendTextMessageAsync(_options.SupportChatId, text);
        }
    }
}