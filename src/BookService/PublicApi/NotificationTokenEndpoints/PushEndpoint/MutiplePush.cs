using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Entities.NotificationTokenAggregate;
using Pillow.ApplicationCore.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.NotificationTokenEndpoints.PushEndpoint
{
    [Authorize(Roles = "moderator")]
    public class MutiplePush : BaseAsyncEndpoint<MultiplePushRequest, MultiplePushResponse>
    {
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IAsyncRepository<NotificationToken> _tokenRepository;

        public MutiplePush(IPushNotificationService pushNotificationService,
            IAsyncRepository<NotificationToken> tokenRepository)
        {
            _pushNotificationService = pushNotificationService;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("api/notificationTokens/push")]
        [SwaggerOperation(
            Summary = "Push the message on all notification tokens",
            Description = "Push the message on all notification tokens",
            OperationId = "notificationTokens.MultiplePush",
            Tags = new[] { "NotificationTokens" })
        ]
        public async override Task<ActionResult<MultiplePushResponse>> HandleAsync([FromForm]MultiplePushRequest request,
            CancellationToken cancellationToken)
        {
            var response = new MultiplePushResponse(request.CorrelationId());
            var tokens = (await _tokenRepository.ListAllAsync()).Select(t => t.Token);

            var result = await _pushNotificationService.Push(tokens.ToArray(), request.Message).ConfigureAwait(false);

            response.NumberOfTotal = result.NumberOfTotal;
            response.NumberOfSuccess = result.NumberOfSuccess;
            response.NumberOfErrors= result.NumberOfErrors;

            return response;
        }
    }
}