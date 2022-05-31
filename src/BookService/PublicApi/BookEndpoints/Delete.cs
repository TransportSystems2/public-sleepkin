using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Constants;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.BookEndpoints
{
    [Authorize (Roles=AuthorizationConstants.Roles.Moderator)]
    public class Delete : BaseAsyncEndpoint<DeleteBookRequest, DeleteBookResponse>
    {
        private readonly IBookRepository _bookRepository;

        public Delete(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpDelete("api/books/{BookCode}")]
        [SwaggerOperation(
            Summary = "Deletes a Book",
            Description = "Deletes a Book",
            OperationId = "book.Delete",
            Tags = new[] { "BookEndpoints" })
        ]
        public override async Task<ActionResult<DeleteBookResponse>> HandleAsync([FromRoute]DeleteBookRequest request,
            CancellationToken cancellationToken)
        {
            DeleteBookResponse response = new DeleteBookResponse(request.CorrelationId());

            Book bookToDelete = await _bookRepository.GetByCodeAsync(request.BookCode);
            if (bookToDelete is null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteAsync(bookToDelete);

            return Ok(response);
        }
    }
}