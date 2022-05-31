using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Entities.UserAccountAggregate;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Specifications;
using Pillow.PublicApi.TrackEndpoints;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Pillow.PublicApi.UsersEndpoints.FavoriteBooks
{
    [Authorize]
    public class ListPagedFavoriteBooks : BaseAsyncEndpoint<ListPagedFavoriteBookRequest, ListPagedFavoriteBookResponse>
    {
        private readonly IAsyncRepository<UserAccount> _userAccountRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IMapper _mapper;

        public ListPagedFavoriteBooks(IAsyncRepository<UserAccount> userAccountRepository,
            IBookRepository bookRepository,
            IUriComposer uriComposer,
            IMapper mapper)
        {
            _userAccountRepository = userAccountRepository;
            _bookRepository = bookRepository;
            _uriComposer = uriComposer;
            _mapper = mapper;
        }

        [HttpGet("api/users/{UserName}/books/favorites/")]
        [SwaggerOperation(
            Summary = "List of favorite books (paged)",
            Description = "List of favorite books (paged)",
            OperationId = "users.pagedFavoriteBooks",
            Tags = new[] { "UsersEndpoints.FavoriteBooks" })
        ]
        public override async Task<ActionResult<ListPagedFavoriteBookResponse>> HandleAsync(
            [FromQuery]ListPagedFavoriteBookRequest request,
            CancellationToken cancellationToken)
        {
            var response = new ListPagedFavoriteBookResponse(request.CorrelationId());
            
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(userName))
                return Unauthorized(userName);
            
            if (request.UserName != userName)
                return Forbid(userName);
            
            var userAccountSpec = new UserAccountSpecification(userName);
            UserAccount userAccount = await _userAccountRepository.FirstOrDefaultAsync(userAccountSpec);

            if (userAccount == null)
                userAccount = await _userAccountRepository.AddAsync(new UserAccount(userName));

            string[] favoriteBookCodes = userAccount.Favorites
                .Select(fb => fb.BookCode).ToArray();

            var filterSpec = new FavoriteBookPaginatedSpecification(favoriteBookCodes, request.Tags);
            int totalItems = await _bookRepository.CountAsync(filterSpec);

            var pagedSpec = new FavoriteBookPaginatedSpecification(
                bookCodes: favoriteBookCodes,
                skip: request.Offset,
                take: request.Limit,
                tags: request.Tags);

            var items = await _bookRepository.ListAsync(pagedSpec);

            response.Items.AddRange(items.Select(_mapper.Map<FavoriteBookDto>));
            foreach (FavoriteBookDto item in response.Items)
            {
                foreach(TrackDto track in item.Tracks)
                {
                    track.Path = _uriComposer.ComposeTrackUri(item.Code, track.Code, track.Format);
                }

                TrackDto trailer = item.Trailer;
                if (trailer != null)
                {
                    trailer.Path = _uriComposer.ComposeTrackUri(item.Code, trailer.Code, trailer.Format);
                }

                item.CoverUri = _uriComposer.ComposePicUri(item.CoverUri);
            }

            response.Total = totalItems;

            return Ok(response);
        }
    }
}
