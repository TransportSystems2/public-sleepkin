using System.Threading;
using System.Threading.Tasks;
using Pillow.ApplicationCore.Services.Subscriptions.Contracts;

namespace Pillow.ApplicationCore.Services.Subscriptions.Interfaces
{
    public interface ISubscriptionService
    {
        Task<MultipleUpdate.Response> UpdateSubscriptions(MultipleUpdate.Request request,
            CancellationToken cancellationToken);

        Task<Update.Response> UpdateSubscription(Update.Request request,
            CancellationToken cancellationToken,
            bool isUseSandboxEnvironment = false);
    }
}