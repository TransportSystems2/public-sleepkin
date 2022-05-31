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
        public record PostUserCodeRequest([NotNull] string Code, [NotNull] string UserName);

        [Authorize]
        [HttpPost("api/v1/promocode/{UserName}/{Code}")]
        [SwaggerOperation(
            Summary = "Use promo code by user",
            Description = "ErrorMessages: <br/>" +
                          " 404 PromoCodeNotFound - there are not found promo code <br/>" +
                          " 422 PromoCodeAlreadyUsed - the user has already used this promo code <br/>" +
                          " 422 InactivePromoCode - inactive promo code <br/>" +
                          " 422 PromoCodeExceededUsageLimit - the promo code usage limit has been exceeded",
            OperationId = "promocode.useCodeByUser",
            Tags = new[] { "PromoCodeEndpoints" })
        ]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> UseCodeByUser([FromRoute] PostUserCodeRequest request)
        {
            try
            {
                var tokenUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrWhiteSpace(tokenUserName))
                    return Unauthorized(tokenUserName);
                if (tokenUserName != request.UserName)
                    return Forbid($"Different name from url:{request.UserName} and token:{tokenUserName}");

                await _promoCodeService.UsePromoCode(tokenUserName, request.Code);
                return Created($"api/v1/promococe/{request.UserName}", new {});
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