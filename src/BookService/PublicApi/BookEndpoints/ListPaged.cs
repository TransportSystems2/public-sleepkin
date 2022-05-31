using System;
using System.Collections.Generic;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Specifications;
using Pillow.PublicApi.TrackEndpoints;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pillow.ApplicationCore.Constants;
using Pillow.ApplicationCore.Enums;
using Pillow.ApplicationCore.Extensions;

namespace Pillow.PublicApi.BookEndpoints
{
    [Authorize]
    public class ListPaged : BaseAsyncEndpoint<ListPagedBookRequest, ListPagedBookResponse>
    {
        /// <summary>
        /// Ограничение бесплатных книг для пользователей без посписки
        /// </summary>
        public class UserFreeBookLimitOptions
        {
            /// <summary>
            /// Для пользователей зарегестрированных с
            /// </summary>
            public DateTime FromRegisterUserDate { get; set; }
            
            /// <summary>
            /// Количество доступных бесплатных книг
            /// </summary>
            public int Limit { get; set; }
        }
        
        private readonly IBookRepository _bookRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IMapper _mapper;
        private readonly UserFreeBookLimitOptions _freeBookLimitOptions;

        public ListPaged(IBookRepository bookRepository,
            IUriComposer uriComposer,
            IMapper mapper,
            IOptions<UserFreeBookLimitOptions> freeBookLimitOptions)
        {
            _bookRepository = bookRepository;
            _uriComposer = uriComposer;
            _mapper = mapper;
            _freeBookLimitOptions = freeBookLimitOptions.Value;
        }

        [HttpGet("api/books")]
        [SwaggerOperation(
            Summary = "List Books (paged)",
            Description = "List Books (paged)",
            OperationId = "books.ListPaged",
            Tags = new[] { "BookEndpoints" })
        ]
        public override async Task<ActionResult<ListPagedBookResponse>> HandleAsync(
            [FromQuery]ListPagedBookRequest request,
            CancellationToken cancellationToken)
        {
            var response = new ListPagedBookResponse(request.CorrelationId());

            var userHasSubscription = User.HasClaim(ClaimConstans.HasSubscription, "true");

            BaseBookPaginatedSpecification totalItemsSpecification = userHasSubscription
                    ? new BookPaginatedSpecificationForUsersWithSubscription(request.Tags, request.IncludeNotPublished)
                    : new BookPaginatedSpecificationForUsersWithoutSubscription(request.Tags, request.IncludeNotPublished);
            int totalItems = await _bookRepository.CountAsync(totalItemsSpecification);

            BaseBookPaginatedSpecification pagedSpec = userHasSubscription
                ? new BookPaginatedSpecificationForUsersWithSubscription(request.Offset, request.Limit, request.Tags, request.IncludeNotPublished)
                : new BookPaginatedSpecificationForUsersWithoutSubscription(request.Offset, request.Limit, request.Tags, request.IncludeNotPublished);
            var items = await _bookRepository.ListAsync(pagedSpec);

            var registerUserDate = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimConstans.Created))
                ?.Value.FromString();

            var dtoBooks = items.Select(_mapper.Map<BookDto>).ToList();

            if (!userHasSubscription
                && registerUserDate.HasValue && registerUserDate >= _freeBookLimitOptions.FromRegisterUserDate)
            {
                dtoBooks = LimitNumberOfFreeBooks(dtoBooks, ref totalItems);
            }

            response.Items.AddRange(dtoBooks);
            foreach (BookDto item in response.Items)
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

        private List<BookDto> LimitNumberOfFreeBooks(List<BookDto> dtoBooks, ref int totalItems)
        {
            var freeBooks = dtoBooks.Where(book => book.AccessLevel == AccessLevel.Free
                                                   && !book.Tags.Any());
            var exceptedBooks = freeBooks
                .TakeLast(freeBooks.Count() - _freeBookLimitOptions.Limit);
            dtoBooks = dtoBooks.Except(exceptedBooks).ToList();
            totalItems -= _freeBookLimitOptions.Limit;

            return dtoBooks;
        }
    }
}
