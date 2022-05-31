using System;
using Ardalis.Specification;
using Pillow.ApplicationCore.Entities.SubscriptionAggregate;

namespace Pillow.ApplicationCore.Services.Subscriptions.Specifications
{
    public class ActiveSubscriptionSpecification : Specification<Subscription>
    {
        public ActiveSubscriptionSpecification(bool isOnlyActive, DateTime gracePeriod)
        {
            if (isOnlyActive)
            {
                Query.Where(subscription =>
                    subscription.ExpiredDate >= gracePeriod
                    && (subscription.CancellationDate == DateTime.MinValue)
                );
            }

            Query.Where(subscription =>
                subscription.ReceiptData != null
                && subscription.ReceiptData != "");
        }
    }
}