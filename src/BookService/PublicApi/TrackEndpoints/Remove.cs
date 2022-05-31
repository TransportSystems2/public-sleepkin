using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Constants;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Specifications;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.TrackEndpoints
{
    [Authorize (Roles=AuthorizationConstants.Roles.Moderator)]
    public class Delete : BaseAsyncEndpoint<RemoveTrackRequest, RemoveTrackResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUriComposer _uriComposer;

        public Delete(IBookRepository bookRepository,
            IUriComposer uriComposer)
        {
            _bookRepository = bookRepository;
            _uriComposer = uriComposer;
        }

        [HttpDelete("api/books/{BookCode}/tracks/{TrackCode}")]
        [SwaggerOperation(
            Summary = "Remove a track file from book",
            Description = "Remove a track file from book",
            OperationId = "tracks.Remove",
            Tags = new[] { "TrackEndpoints" })
        ]
        public override async Task<ActionResult<RemoveTrackResponse>> HandleAsync(
            [FromRoute]RemoveTrackRequest request,
            CancellationToken cancellationToken)
        {
            RemoveTrackResponse response = new RemoveTrackResponse(request.CorrelationId());

            BookSpecification spec = new BookSpecification(request.BookCode);
            Book book = await _bookRepository.FirstOrDefaultAsync(spec);
            if (book is null) return NotFound($"BookdId: {request.BookCode} doesn't exist ");

            Track track = book.Tracks.FirstOrDefault(t => t.Code == request.TrackCode);
            if (track is null) return NotFound($"TrackId: {request.TrackCode} dosen't exist");

            string trackPath = _uriComposer.ComposeTrackFilePath(track.Code, track.Format);
            book.RemoveTrack(track, trackPath, request.IsHardRemove);
            await _bookRepository.UpdateAsync(book);

            return Ok(response);
        }
    }
}