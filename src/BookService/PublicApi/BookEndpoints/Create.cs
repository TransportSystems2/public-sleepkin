using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Constants;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.BookEndpoints
{
    [Authorize (Roles=AuthorizationConstants.Roles.Moderator)]
    public class Create : BaseAsyncEndpoint<CreateBookRequest, CreateBookResponse>
    {
        private readonly IBookRepository _bookRepository;

        private readonly IMapper _mapper;

        public Create(IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpPost("api/books")]
        [SwaggerOperation(
            Summary = "Creates a new Book",
            Description = "Creates a new Book",
            OperationId = "book.create",
            Tags = new[] { "BookEndpoints" })
        ]
        public override async Task<ActionResult<CreateBookResponse>> HandleAsync(CreateBookRequest request,
            CancellationToken cancellationToken)
        {
            var response = new CreateBookResponse(request.CorrelationId());

            Book newBook = new Book(request.Code, request.Title, request.CoverUri, request.AccessLevel, request.Author);
            newBook = await _bookRepository.AddAsync(newBook);

            BookDto dto = _mapper.Map<BookDto>(newBook);
            response.Book = dto;

            return response;
        }
    }
}