using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pillow.ApplicationCore.Interfaces;
using System.Text.Json.Serialization;
using System.Threading;

namespace Pillow.Infrastructure.Services
{
    public class AppStoreSubscriptionService : IAppStoreSubscriptionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly AppStoreSubcriptionOptions _options;

        public AppStoreSubscriptionService(HttpClient httpClient,
            ILogger<AppStoreSubscriptionService> logger,
            IOptions<AppStoreSubcriptionOptions> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<VerifyReceiptResponse> VerifyReceipt(string receiptDate,
            bool excludeOldTransaction,
            CancellationToken cancellationToken,
            bool isSandboxEnvironment = false)
        {
            try
            {
                Guard.Against.NullOrWhiteSpace(receiptDate, nameof(receiptDate));
                Guard.Against.NullOrWhiteSpace(_options.SharedSecret, nameof(_options.SharedSecret));

                var request = new VerifeReceiptRequest
                {
                    ReceiptDate = receiptDate,
                    Password = _options.SharedSecret,
                    ExcludeOldTransaction = excludeOldTransaction
                };

                var receiptString = JsonSerializer.Serialize(request);
                var stringContent = new StringContent(receiptString, Encoding.UTF8, "application/json");
                
                var requestUri = isSandboxEnvironment
                    ? new Uri(_options.SandboxAddress)
                    : new Uri(_options.BuyAddress);
                
                using var response = await _httpClient.PostAsync(new Uri(requestUri, "verifyReceipt"),
                    stringContent,
                    cancellationToken);
                
                response.EnsureSuccessStatusCode();
                var options = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    Converters = { new BooleanConverter() }
                };

                return JsonSerializer.Deserialize<VerifyReceiptResponse>(
                    await response.Content.ReadAsStringAsync(cancellationToken), options);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error of verify receipt");
                throw;
            }
        }
    }

    public class AppStoreSubcriptionOptions
    {
        public string BuyAddress { get; set; }
        public string SandboxAddress { get; set; }
        public string SharedSecret { get; set; }
    }
}