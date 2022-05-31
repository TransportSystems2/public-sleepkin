using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Http;
using PublicApi.UsersEndpoints.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Pillow.ApplicationCore.Entities.UserAccountAggregate;
using Pillow.ApplicationCore.Interfaces;
using System.Security.Claims;
using System.Threading;
using Pillow.ApplicationCore.Services.Subscriptions.Interfaces;

namespace Pillow.PublicApi.UsersEndpoints.Subscriptions
{
    public class UpdateSubscriptions : BaseAsyncEndpoint<UpdateSubscriptionRequest, UpdateSubscriptionsResponse>
    {
        private readonly IAsyncRepository<UserAccount> _userAccountRepository;
        private readonly ISubscriptionService _subscriptionService;

        public UpdateSubscriptions(IAsyncRepository<UserAccount> userAccountRepository,
            ISubscriptionService subscriptionService)
        {
            _userAccountRepository = userAccountRepository;
            _subscriptionService = subscriptionService;
        }

        [HttpPut("api/users/{UserName}/subscriptions/")]
        [SwaggerOperation(
            Summary = "Update user subscription",
            Description = "Update user subscription",
            OperationId = "users.subscriptions",
            Tags = new[] { "UsersEndpoints.Subscriptions" })
        ]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public override async Task<ActionResult<UpdateSubscriptionsResponse>> HandleAsync([FromForm]UpdateSubscriptionRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = new UpdateSubscriptionsResponse(request.CorrelationId());

            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(userName))
                return Unauthorized(userName);

            if (request.UserName != userName)
                return Forbid(userName);

            var updateRequest = new ApplicationCore.Services.Subscriptions.Contracts.Update.Request
            {
                UserName = userName,
                ReceiptData = request.ReceiptData,
                ExcludeOldTransaction = request.ExcludeOldTransaction
            };
            
            var updateResponse = await _subscriptionService.UpdateSubscription(updateRequest, cancellationToken);

            if (updateResponse.Status == 0)
            {
                return Ok(response);
            }

            return UnprocessableEntity(response);
        }
    }
}