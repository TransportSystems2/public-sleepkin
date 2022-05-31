using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pillow.ApplicationCore.Entities.SubscriptionAggregate;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Services.Subscriptions.Contracts;
using Pillow.ApplicationCore.Services.Subscriptions.Interfaces;
using Pillow.ApplicationCore.Services.Subscriptions.Specifications;

namespace Pillow.ApplicationCore.Services.Subscriptions.Implementation
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IAppStoreSubscriptionService _appStoreSubscriptionService;
        private readonly IAsyncRepository<Subscription> _subscriptionRepository;
        private readonly ILogger<SubscriptionService> _logger;
        private static readonly SemaphoreSlim DbContextSemaphoreSlim = new (1, 1);

        public SubscriptionService(IAppStoreSubscriptionService appStoreSubscriptionService,
            IAsyncRepository<Subscription> subscriptionRepository,
            ILogger<SubscriptionService> logger)
        {
            _appStoreSubscriptionService = appStoreSubscriptionService;
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }

        public async Task<MultipleUpdate.Response> UpdateSubscriptions(MultipleUpdate.Request request,
            CancellationToken cancellationToken)
        {
            var gracePeriodDateTime = DateTime.UtcNow - request.GracePeriod;
            var specification = new ActiveSubscriptionSpecification(request.IsOnlyActive, gracePeriodDateTime);
            var subscriptions = await _subscriptionRepository.ListAsync(specification);

            var tasks = subscriptions.Select(subscription => UpdateSubscription(
                new Update.Request
                {
                    UserName = subscription.UserAccountUserName,
                    ExcludeOldTransaction = request.ExcludeOldTransaction,
                    ReceiptData = subscription.ReceiptData
                },
                cancellationToken));
            
            var numberOfErrors = 0;
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (AggregateException ex)
            {
                _logger.LogError(ex, "Error during updating subscriptions");
                numberOfErrors = ex.InnerExceptions.Count();
            }

            return new MultipleUpdate.Response
            {
                NumberOfTotal = subscriptions.Count,
                NumberOfErrors = numberOfErrors,
                NumberOfSuccess = subscriptions.Count - numberOfErrors
            };
        }

        public async Task<Update.Response> UpdateSubscription(Update.Request request, CancellationToken cancellationToken, bool isUseSandboxEnvironment = false)
        {
            while (true)
            {
                var verifyResponse = await _appStoreSubscriptionService.VerifyReceipt(request.ReceiptData, request.ExcludeOldTransaction, cancellationToken, isUseSandboxEnvironment);

                var response = new Update.Response {Status = verifyResponse.Status};

                switch (verifyResponse)
                {
                    case {Status: 0}:
                        await DbContextSemaphoreSlim.WaitAsync(cancellationToken);
                        try
                        {
                            var specification = new SubscriptionsByUserSpecification(request.UserName);
                            Subscription subscription =
                                await _subscriptionRepository.FirstOrDefaultAsync(specification);
                            LatestReceiptInfo latestReceiptInfo = verifyResponse.LatestReceiptInfo?.FirstOrDefault();

                            if (latestReceiptInfo == null)
                            {
                                _logger.LogInformation("No information about latestReceipt, probably a new user");
                                break;
                            }

                            if (subscription == null)
                            {
                                subscription = new Subscription(request.UserName,
                                    verifyResponse.LatestReceipt,
                                    latestReceiptInfo);
                                await _subscriptionRepository.AddAsync(subscription);
                            }
                            else
                            {
                                subscription.UpdateReceiptInfo(verifyResponse.LatestReceipt, latestReceiptInfo);
                                await _subscriptionRepository.UpdateAsync(subscription);
                            }
                        }
                        finally
                        {
                            DbContextSemaphoreSlim.Release();
                        }

                        break;

                    // We should use sandbox environment
                    case {Status: 21007}:
                        isUseSandboxEnvironment = true;
                        continue;

                    default:
                        _logger.LogError($"Подписка не прошла верификацию userName:{request.UserName}" +
                                         $"environment:{verifyResponse.Environment}" +
                                         $"status:{verifyResponse.Status}" +
                                         $"receiptData:{request.ReceiptData}");
                        break;
                }

                return response;
            }
        }
    }
}