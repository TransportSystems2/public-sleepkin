using Microsoft.AspNetCore.Mvc;
using PromoCode.API.Services.PromoCodes.Interfaces;

namespace PromoCode.API.Controllers
{
    [ApiController]
    public partial class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }
    }
}