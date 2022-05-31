using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PromoCode.API.Infrastructure;
using PromoCode.API.Infrastructure.Exceptions;
using PromoCode.API.Services.PromoCodes.Interfaces;

namespace PromoCode.API.Services.PromoCodes.Implementation
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly PromoCodeContext _promoCodeContext;
        private readonly ILogger<PromoCodeService> _logger;

        public PromoCodeService(PromoCodeContext promoCodeContext, ILogger<PromoCodeService> logger)
        {
            _promoCodeContext = promoCodeContext;
            _logger = logger;
        }

        public async Task<string> GetGroupByPromoCode(string userName, string promoCode)
        {
            Guard.Against.NullOrWhiteSpace(userName, nameof(userName));
            Guard.Against.NullOrWhiteSpace(promoCode, nameof(promoCode));
            
            var promoCodeItem = await _promoCodeContext.PromoCodeItems
                .Where(pi => pi.PromoCode == promoCode)
                .Include(pi => pi.AppliedItems)
                .FirstOrDefaultAsync();

            if (promoCodeItem == null)
            {
                throw new NotFoundPromoCodeException($"There are not found promoCode: {promoCode}");
            }

            promoCodeItem.Validate(userName);

            return promoCodeItem.SubscriptionsGroup;
        }

        public async Task UsePromoCode(string userName, string promoCode)
        {
            Guard.Against.NullOrWhiteSpace(userName, nameof(userName));
            Guard.Against.NullOrWhiteSpace(promoCode, nameof(promoCode));

            var promoCodeItem = await _promoCodeContext.PromoCodeItems
                .Where(pi => pi.PromoCode == promoCode)
                .Include(pi => pi.AppliedItems)
                .FirstOrDefaultAsync();

            if (promoCodeItem == null)
            {
                throw new NotFoundPromoCodeException($"There are not found promoCode: {promoCode}");
            }
            
            promoCodeItem.Use(userName);
            await _promoCodeContext.SaveChangesAsync();
        }
    }
}