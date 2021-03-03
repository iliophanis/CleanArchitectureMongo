using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Validators;
using Domain.Entities;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace Application.Entities.Commands
{
    public class DeleteEntity
    {
        #region Command
        public class Command : IRequest
        {
            public string Id { get; set; }
        }
        #endregion

        #region Validator
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(r => r.Id).ValidObjectId();
            }
        }
        #endregion

        #region Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IContext _context;
            public Handler(IContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var filter = Builders<Entity>.Filter.Eq(e => e.Id, request.Id);

                var entity = (await _context.Entities.FindAsync(filter)).FirstOrDefault();
                if (entity == null)
                    throw new NotFoundException(typeof(Entity).ToString(), request.Id);

                await _context.Entities.DeleteOneAsync(filter);

                return Unit.Value;
            }
        }
        #endregion
    }
}