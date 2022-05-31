using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Constants;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Specifications;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.BookEndpoints
{
    [Authorize (Roles=AuthorizationConstants.Roles.Moderator)]
    public class Update : BaseAsyncEndpoint<UpdateBookRequest, UpdateBookResponse>
    {
        private readonly IBookRepository _bookRepository;

        public Update(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpPatch("api/books/{BookCode}")]
        [SwaggerOperation(
            Summary = "Update a Book",
            Description = "Update a Book",
            OperationId = "book.Update",
            Tags = new[] { "BookEndpoints" })
        ]
        public override async Task<ActionResult<UpdateBookResponse>> HandleAsync([FromForm]UpdateBookRequest request,
            CancellationToken cancellationToken)
        {
            var response = new UpdateBookResponse(request.CorrelationId());
            var updateSpec = new BookSpecification(request.BookCode);
            var book = await _bookRepository.FirstOrDefaultAsync(updateSpec);

            if (book == null) return NotFound();

            if (request.IsPublished.HasValue)
            {
                if (request.IsPublished.Value)
                    book.Publish();
                else
                    book.CancelPublication();
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                book.UpdateTitle(request.Title);
            }

            await _bookRepository.UpdateAsync(book);

            return Ok(response);
        }
    }
}