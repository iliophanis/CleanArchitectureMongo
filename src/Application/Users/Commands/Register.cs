using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands
{
    public class Register
    {
        #region Command
        public class Command : IRequest<string>
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }
        #endregion

        #region Validator
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(r => r.EmailAddress).NotEmpty().EmailAddress();
                RuleFor(r => r.Password).NotEmpty();
            }
        }
        #endregion

        #region Handler
        public class Handler : IRequestHandler<Command, string>
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

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var (result, userId) = await _identityService.CreateUserAsync(request.EmailAddress, request.Password);
                if (!result.Succeeded)
                    throw new BadRequestException("");

                return _tokenGenerator.CreateToken(userId);
            }
        }
        #endregion 
    }
}