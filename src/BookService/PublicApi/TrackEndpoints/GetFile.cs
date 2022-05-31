using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Constants;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Enums;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Specifications;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IContentTypeProvider = Pillow.ApplicationCore.Interfaces.IContentTypeProvider;

namespace Pillow.PublicApi.TrackEndpoints
{
    [Authorize]
    public class GetFile : BaseAsyncEndpoint
    {
        private const string AccelRedirectRoute = "content";
        private readonly IBookRepository _bookRepository;

        private readonly IUriComposer _uriComposer;
        private readonly IContentTypeProvider _contentTypeProvider;

        public GetFile(IBookRepository bookRepository,
            IUriComposer uriComposer,
            IContentTypeProvider contentTypeProvider)
        {
            _bookRepository = bookRepository;
            _uriComposer = uriComposer;
            _contentTypeProvider = contentTypeProvider;
        }

        [HttpGet("api/books/{BookCode}/tracks/{TrackCode}/{TrackFormat}")]
        [HttpHead("api/books/{BookCode}/tracks/{TrackCode}/{TrackFormat}")]
        [SwaggerOperation(
            Summary = "Get a track file by Id and Format",
            Description = "Gets a track file by Id and Format",
            OperationId = "tracks.GetFile",
            Tags = new[] { "TrackEndpoints" })
        ]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 6000)]
        public async Task<IActionResult> HandleAsync([FromRoute] GetFileTrackRequest request,
            CancellationToken cancellationToken)
        {
            BookSpecification spec = new BookSpecification(request.BookCode);
            Book book = await _bookRepository.FirstOrDefaultAsync(spec);

            if (book is null)
            {
                return NotFound();
            }

            Track track = book.Tracks.FirstOrDefault(t => t.Code == request.TrackCode);

            if (track is null)
            {
                return NotFound();
            }
            
            if (!HasAccess(track, User))
            {
                if (User.Identity.IsAuthenticated)
                {
                    return Forbid();
                }

                return Unauthorized();
            }
            
            string filePath = _uriComposer.GetTrackPath(track.Code, track.Format);
            _contentTypeProvider.TryGetContentType(filePath, out string contentType);
            
            HttpContext.Response.Headers.Add("ContentType", contentType);
            HttpContext.Response.Headers.Add("X-Accel-Redirect", $"/{AccelRedirectRoute}/tracks/{filePath}");

            return Ok();
        }

        private bool HasAccess(Track track, ClaimsPrincipal user)
        {
            Guard.Against.Null(track, nameof(track));
            Guard.Against.Null(user, nameof(user));

            bool freeBook = track.Book?.AccessLevel == AccessLevel.Free;
            if (track.IsTrailer || freeBook)
            {
                return true;
            }

            return user.HasClaim(ClaimConstans.HasSubscription, "true");
        }
    }
}