using System.Collections.Generic;
using System.Linq;
using PromoCode.API.Infrastructure.Exceptions;

namespace PromoCode.API.Model
{
    public class PromoCodeItem
    {
        public PromoCodeItem()
        {
        }

        public string PromoCode { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int Limit { get; set; } = -1;

        public string SubscriptionsGroup { get; set; }

        private readonly List<AppliedPromoCodeItem> _appliedItems = new();
        public virtual IReadOnlyCollection<AppliedPromoCodeItem> AppliedItems => _appliedItems.AsReadOnly();

        public void Use(string userName)
        {
            Validate(userName);

            AppliedPromoCodeItem appliedPromoCode = new() { PromoCode = PromoCode, UserName = userName };
            _appliedItems.Add(appliedPromoCode);
        }

        public void Validate(string userName)
        {
            if (!IsActive)
            {
                throw new InactivePromoCodeException($"Inactive promo code:{PromoCode}");
            }
            
            if (_appliedItems.Any(promoCode => promoCode.PromoCode == PromoCode && promoCode.UserName == userName))
            {
                throw new AlreadyUsedPromoCodeException($"This promo code:${PromoCode} already used user:{userName}");
            }

            if (Limit >= 0 && _appliedItems.Count >= Limit)
            {
                throw new ExceededUsageLimitException(
                    $"This promo code:${PromoCode} used is ${_appliedItems.Count} out of {Limit}");
            }
        }
    }
}