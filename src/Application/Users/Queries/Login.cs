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
            public string Username { get; set; }
            public string Password { get; set; }
        }
        #endregion

        #region Validator
        public class QueryValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                RuleFor(r => r.Username).NotEmpty();
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
                var (result, userId) = await _identityService.Login(request.Username, request.Password);
                if (!result.Succeeded)
                    throw new BadRequestException("");

                return _tokenGenerator.CreateToken(userId);
            }
        }
        #endregion
    }
}