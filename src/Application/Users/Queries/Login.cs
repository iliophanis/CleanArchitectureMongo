using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands
{
    public class Login
    {
        #region Query
        public class Query : IRequest<string>
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }
        #endregion

        #region Validator
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(r => r.EmailAddress).NotEmpty().EmailAddress();
                RuleFor(r => r.Password).NotEmpty();
            }
        }
        #endregion

        #region Handler
        public class Handler : IRequestHandler<Query, string>
        {
            private readonly IContext _context;
            private readonly IIdentityService _identityService;
            private readonly ITokenGenerator _tokenGenerator;

            public Handler(IContext context, IIdentityService identityService, ITokenGenerator tokenGenerator)
            {
                _context = context;
                _identityService = identityService;
                _tokenGenerator = tokenGenerator;
            }

            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                var (result, userId) = await _identityService.LoginUserAsync(request.EmailAddress, request.Password);
                if (!result.Succeeded)
                    throw new BadRequestException(string.Concat(result.Errors));

                return _tokenGenerator.CreateToken(userId);
            }
        }
        #endregion
    }
}