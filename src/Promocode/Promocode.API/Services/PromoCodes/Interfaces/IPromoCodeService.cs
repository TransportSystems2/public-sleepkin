using System.Threading.Tasks;

namespace PromoCode.API.Services.PromoCodes.Interfaces
{
    public interface IPromoCodeService
    {
        Task<string> GetGroupByPromoCode(string userName, string promoCode);
        Task UsePromoCode(string userName, string promoCode);
    }
}