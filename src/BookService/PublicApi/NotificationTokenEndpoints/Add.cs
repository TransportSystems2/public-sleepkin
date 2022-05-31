using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Entities.NotificationTokenAggregate;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Specifications;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.NotificationTokenEndpoints
{
    [Authorize]
    public class Add : BaseAsyncEndpoint<AddNotificationTokenRequest, AddNotificationTokenResponse>
    {
        private readonly IAsyncRepository<NotificationToken> _tokenRepository;

        public Add(IAsyncRepository<NotificationToken> tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        [HttpPost("api/notificationTokens")]
        [SwaggerOperation(
            Summary = "Add a notification token",
            Description = "Add a notification token",
            OperationId = "notificationTokens.Add",
            Tags = new[] { "NotificationTokens" })
        ]
        [ProducesResponseType(typeof(AddNotificationTokenRequest), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async override Task<ActionResult<AddNotificationTokenResponse>> HandleAsync(AddNotificationTokenRequest request,
            CancellationToken cancellationToken)
        {
            var response = new AddNotificationTokenResponse(request.CorrelationId());

            NotificationTokenFilterSpecification spec =
                new NotificationTokenFilterSpecification(request.Token);

            if (await _tokenRepository.CountAsync(spec) > 0)
            {
                return Conflict();
            }

            NotificationToken newToken = new NotificationToken(request.Token,
                request.PlatformKind, request.DeviceName, request.UserId);

            await _tokenRepository.AddAsync(newToken);

            return new CreatedResult($"api/notificationTokens/{newToken.Token}", newToken);
        }
    }
}