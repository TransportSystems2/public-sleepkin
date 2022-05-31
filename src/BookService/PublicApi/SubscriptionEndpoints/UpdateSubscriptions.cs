using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Services.Subscriptions.Contracts;
using Pillow.ApplicationCore.Services.Subscriptions.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.SubscriptionEndpoints
{
    public sealed class Request : BaseRequest
    {
        public bool IsOnlyActive { get; set; } = true;
        
        public bool ExcludeOldTransaction { get; set; } = true;

        public TimeSpan GracePeriod { get; set; } = TimeSpan.FromDays(7);
    }

    public sealed class Response : BaseResponse
    {
        public Response()
        {
        }

        public Response(Guid correlationId) : base(correlationId)
        {
        }
        
        public int NumberOfTotal { get; set; }

        public int NumberOfSuccess { get; set; }

        public int NumberOfErrors { get; set; }
    }

    [Authorize(Roles = "moderator")]
    public class CheckSubscriptions : BaseAsyncEndpoint<Request, Response>
    {
        private readonly ISubscriptionService _subscriptionService;

        public CheckSubscriptions(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("api/subscriptions/update")]
        [SwaggerOperation(
            Summary = "Update all subscriptions",
            Description = "Update all subscriptions",
            OperationId = "subscriptions.Update",
            Tags = new[] { "SubscriptionsEndpoints" })
        ]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public override async Task<ActionResult<Response>> HandleAsync([FromQuery]Request request,
            CancellationToken cancellationToken)
        {
            var response = new Response(request.CorrelationId());
            var updateResponse = await _subscriptionService.UpdateSubscriptions(
                new MultipleUpdate.Request
                {
                    ExcludeOldTransaction = request.ExcludeOldTransaction,
                    IsOnlyActive = request.IsOnlyActive,
                    GracePeriod = request.GracePeriod
                },
                cancellationToken);

            response.NumberOfTotal = updateResponse.NumberOfTotal;
            response.NumberOfSuccess = updateResponse.NumberOfSuccess;
            response.NumberOfErrors = updateResponse.NumberOfErrors;

            return Ok(response);
        }
    }
}