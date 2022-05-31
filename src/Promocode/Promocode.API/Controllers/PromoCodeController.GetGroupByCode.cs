using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromoCode.API.Infrastructure.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace PromoCode.API.Controllers
{
    public partial class PromoCodeController
    {
        public record GroupRequest([NotNull] string Code);

        public record GroupResponse([CanBeNull] string GroupSubscription = null, [CanBeNull] string Message = null);

        [Authorize]
        [HttpGet("api/v1/promocode/{Code}/group")]
        [SwaggerOperation(
            Summary = "Get a subscription group by promo code",
            Description = "ErrorMessages: <br/>" +
                          " 404 PromoCodeNotFound - there are not found promo code <br/>" +
                          " 422 PromoCodeAlreadyUsed - the user has already used this promo code <br/>" +
                          " 422 InactivePromoCode - inactive promo code <br/>" +
                          " 422 PromoCodeExceededUsageLimit - the promo code usage limit has been exceeded",
            OperationId = "promocode.getGroupByPromoCode",
            Tags = new[] { "PromoCodeEndpoints" })
        ]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> GetGroupByPromoCode([FromRoute] GroupRequest request)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrWhiteSpace(userName))
                    return Unauthorized(userName);

                var subscriptionGroup = await _promoCodeService.GetGroupByPromoCode(userName, request.Code);
                return Ok(new GroupResponse(subscriptionGroup));
            }
            catch (NotFoundPromoCodeException)
            {
                return NotFound(new GroupResponse(Message: "PromoCodeNotFound"));
            }
            catch (AlreadyUsedPromoCodeException)
            {
                return UnprocessableEntity(new GroupResponse(Message: "PromoCodeAlreadyUsed"));
            }
            catch (InactivePromoCodeException)
            {
                return UnprocessableEntity(new GroupResponse(Message: "InactivePromoCode"));
            }
            catch (ExceededUsageLimitException)
            {
                return UnprocessableEntity(new GroupResponse(Message: "PromoCodeExceededUsageLimit"));
            }
        }
    }
}