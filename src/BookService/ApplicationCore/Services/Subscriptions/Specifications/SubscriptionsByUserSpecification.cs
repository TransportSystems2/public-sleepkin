using Ardalis.Specification;
using Pillow.ApplicationCore.Entities.SubscriptionAggregate;

namespace Pillow.ApplicationCore.Services.Subscriptions.Specifications
{
    public sealed class SubscriptionsByUserSpecification : Specification<Subscription>
    {
        public SubscriptionsByUserSpecification(string userName)
        {
            Query.Where(subscription => subscription.UserAccountUserName == userName);
        }
    }
}