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

namespace Pillow.PublicApi.UsersEndpoints.PreorderBooks
{
    [Authorize]
    public class AddPreorderBook : BaseAsyncEndpoint<AddPreorderBookRequest, AddPreorderBookResponse>
    {
        private readonly IAsyncRepository<UserAccount> _userAccountRepository;
        private readonly IBookRepository _bookRepository;

        public AddPreorderBook(IAsyncRepository<UserAccount> userAccountRepository,
            IBookRepository bookRepository)
        {
            _userAccountRepository = userAccountRepository;
            _bookRepository = bookRepository;
        }

        [HttpPost("api/users/{UserName}/books/preorders/")]
        [SwaggerOperation(
            Summary = "Pre-order a user book",
            Description = "Pre-order a user book",
            OperationId = "users.pre-orderBooks",
            Tags = new[] { "UsersEndpoints.PreorderBooks" })
        ]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public override async Task<ActionResult<AddPreorderBookResponse>> HandleAsync([FromForm]AddPreorderBookRequest request,
            CancellationToken cancellationToken)
        {
            var response = new AddPreorderBookResponse(request.CorrelationId());

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
               userAccount.AddPreorderBook(book);
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