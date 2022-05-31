using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace PublicApi.NotificationTokenEndpoints.PushEndpoint
{
    [Authorize(Roles = "moderator")]
    public class Push : BaseAsyncEndpoint<PushRequest, PushResponse>
    {
        private readonly IPushNotificationService _pushNotificationService;

        public Push(IPushNotificationService pushNotificationService)
        {
            _pushNotificationService = pushNotificationService;
        }

        [HttpPost("api/notificationTokens/{Token}/push")]
        [SwaggerOperation(
            Summary = "Push the message on notification token",
            Description = "Push the message on notification token",
            OperationId = "notificationTokens.SinglePush",
            Tags = new[] { "NotificationTokens" })
        ]
        public async override Task<ActionResult<PushResponse>> HandleAsync([FromForm]PushRequest request,
            CancellationToken cancellationToken)
        {
            var response = new PushResponse(request.CorrelationId());

            var result = await _pushNotificationService.Push(new [] { request.Token }, request.Message)
                .ConfigureAwait(false);
                
            response.NumberOfTotal = result.NumberOfTotal;
            response.NumberOfSuccess = result.NumberOfSuccess;
            response.NumberOfErrors= result.NumberOfErrors;

            return response;
        }
    }
}