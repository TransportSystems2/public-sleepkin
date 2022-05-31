using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.SuggestionEndpoints
{
    [Authorize]
    public class Add : BaseAsyncEndpoint<AddSuggestionRequest, AddSuggestionResponse>
    {
        private readonly ITelegramBot _telegramBot;

        public Add(ITelegramBot telegramBot)
        {
            _telegramBot = telegramBot;
        }

        [HttpPost("api/suggestions")]
        [SwaggerOperation(
            Summary = "Add a user suggestion",
            Description = "Send a suggestion text to telegram chanel",
            OperationId = "suggestion.Add",
            Tags = new[] { "SuggestionEndpoints" })
        ]
        public override async Task<ActionResult<AddSuggestionResponse>> HandleAsync(AddSuggestionRequest request,
            CancellationToken cancellationToken)
        {
            StringBuilder messageText = new StringBuilder($"Поступило новое предложение от пользователя.\r\n");
            
            if (!string.IsNullOrWhiteSpace(request.Text))
            {
                messageText.AppendLine($"Текст: {request.Text}");
            }

            if (!string.IsNullOrWhiteSpace(request.UserEmail))
            {
                messageText.AppendLine($"Почта: {request.UserEmail}");
            }

            await _telegramBot.SendMessageToSupport(messageText.ToString());

            return Ok();
        }
    }
}