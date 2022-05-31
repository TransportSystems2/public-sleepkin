using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class PushNotificationOptions
    {
        public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(20);

        public string BundleId { get; set; }

        public string TeamId { get; set; }

        public string KeyId { get; set; }

        public string PrivateKey { get; set; }

        public string BaseAddress { get; set; }
    }

    public class PushNotificationService : IPushNotificationService
    {
        private static (string token, DateTime issuedAt) _accessToken;

        private readonly HttpClient _httpClient;
        private readonly PushNotificationOptions _options;
        private readonly IAppleAccessTokenFactory _appleAccessTokenFactory;
        private readonly ILogger<PushNotificationService> _logger;

        public PushNotificationService(HttpClient httpClient,
            IOptions<PushNotificationOptions> options,
            IAppleAccessTokenFactory appleAccessTokenFactory,
            ILogger<PushNotificationService> logger)
        {
            _httpClient = httpClient;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _appleAccessTokenFactory = appleAccessTokenFactory;
            _logger = logger;
        }

        public async Task<PushNotificationResult> Push(string[] tokens, string payload)
        {
            var accessToken = await ObtainAccessToken(_options.Duration);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            var stringContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var tasks = tokens.Select(token => _httpClient.PostAsync($"device/{token}", stringContent));
            
            var numberOfErrors = 0;
            try
            {
                await Task.WhenAll(tasks);
            }
            catch(AggregateException ex)
            {
                _logger.LogError(ex, "Error during pushing notifications");
                numberOfErrors = ex.InnerExceptions.Count();
            }

            return new PushNotificationResult
            {
                NumberOfTotal = tokens.Count(),
                NumberOfSuccess = tokens.Count() - numberOfErrors,
                NumberOfErrors = numberOfErrors
            };
        }

        private async Task<string> ObtainAccessToken(TimeSpan duration)
        {
            if (string.IsNullOrEmpty(_accessToken.token)
                || DateTime.UtcNow > _accessToken.issuedAt.Add(duration))
            {
                var token = await _appleAccessTokenFactory.CreateToken(_options.KeyId,
                    _options.TeamId, _options.PrivateKey);

                _accessToken = (token, DateTime.UtcNow);
            }

            return _accessToken.token;
        }
    }
}