using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pillow.ApplicationCore.Entities.UserAccountAggregate;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Specifications;
using Pillow.Infrastructure.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.UsersEndpoints
{
    public class Create : BaseAsyncEndpoint<CreateUserRequest, CreateUserResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTTokenOptions _jwtTokenOptions;
        private readonly IAsyncRepository<UserAccount> _userAccountRepository;

        public Create(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTTokenOptions> jwtTokenOptions,
            IAsyncRepository<UserAccount> userAccountRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenOptions = jwtTokenOptions.Value ?? throw new ArgumentNullException(nameof(jwtTokenOptions));
            _userAccountRepository = userAccountRepository;
        }

        [HttpPost("api/users")]
        [SwaggerOperation(
            Summary = "Create a user",
            Description = "Create a user",
            OperationId = "users.create",
            Tags = new[] { "UsersEndpoints" })
        ]
        public override async Task<ActionResult<CreateUserResponse>> HandleAsync(CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var response = new CreateUserResponse(request.CorrelationId());

            var newUser = new ApplicationUser { UserName = request.UserName };
            var password = CreateMD5(request.UserName + _jwtTokenOptions.PasswordKey);
            var result = await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                return Conflict(result.Errors);
            }

            var userAccountSpec = new UserAccountSpecification(request.UserName);
            if (await _userAccountRepository.FirstOrDefaultAsync(userAccountSpec) == null)
            {
                await _userAccountRepository.AddAsync(new UserAccount(request.UserName));
            }

            response.Result = result.Succeeded;

            return response;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}