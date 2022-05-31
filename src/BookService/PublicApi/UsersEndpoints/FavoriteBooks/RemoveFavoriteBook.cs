using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Entities.UserAccountAggregate;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Specifications;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.UsersEndpoints.FavoriteBooks
{
    [Authorize]
    public class RemoveFavoriteBook : BaseAsyncEndpoint<RemoveFavoriteBookRequest, RemoveFavoriteBookResponse>
    {
        private readonly IAsyncRepository<UserAccount> _userAccountRepository;
        private readonly IBookRepository _bookRepository;

        public RemoveFavoriteBook(IAsyncRepository<UserAccount> userAccountRepository,
            IBookRepository bookRepository)
        {
            _userAccountRepository = userAccountRepository;
            _bookRepository = bookRepository;
        }

        [HttpDelete("api/users/{UserName}/books/favorites/{BookCode}")]
        [SwaggerOperation(
            Summary = "Remove user favorite book",
            Description = "Remove user favorite book",
            OperationId = "users.removeFavoriteBooks",
            Tags = new[] { "UsersEndpoints.FavoriteBooks" })
        ]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public override async Task<ActionResult<RemoveFavoriteBookResponse>> HandleAsync([FromRoute]RemoveFavoriteBookRequest request,
            CancellationToken cancellationToken)
        {
            var response = new RemoveFavoriteBookResponse(request.CorrelationId());

            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(userName))
                return Unauthorized(userName);
            
            if (request.UserName != userName)
                return Forbid(userName);

            var userAccountSpec = new UserAccountSpecification(userName);
            UserAccount userAccount = await _userAccountRepository.FirstOrDefaultAsync(userAccountSpec);

            if (userAccount == null)
                userAccount = await _userAccountRepository.AddAsync(new UserAccount(userName));

            var bookSpec = new BookSpecification(request.BookCode);
            var book = await _bookRepository.FirstOrDefaultAsync(bookSpec);

            if (book == null)
                return NotFound($"BookCode:{request.BookCode}");

            try
            {
               userAccount.RemoveFavoriteBook(book);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
            
            await _userAccountRepository.UpdateAsync(userAccount);

            return NoContent();
        }
    }
}