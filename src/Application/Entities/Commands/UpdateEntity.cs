using System;
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
    public class UpdateEntity
    {
        #region Command
        public class Command : IRequest<string>
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Age { get; set; }
            public double Factor { get; set; }
        }
        #endregion

        #region Validator
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(r => r.Name).NotEmpty();
                RuleFor(r => r.Description.Length).ExclusiveBetween(0, 255);
                RuleFor(r => r.Age).NotEmpty();
                RuleFor(r => r.Factor).NotEmpty();
            }
        }
        #endregion

        #region Handler
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IContext _context;
            public Handler(IContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var filter = Builders<Entity>.Filter.Eq(e => e.Id, request.Id);

                var entity = (await _context.Entities.FindAsync(filter)).FirstOrDefault();
                if (entity == null)
                    throw new NotFoundException(typeof(Entity).ToString(), request.Id);

                var update = Builders<Entity>.Update
                    .Set(e => e.Name, request.Name)
                    .Set(e => e.Description, request.Description)
                    .Set(e => e.Age, request.Age)
                    .Set(e => e.Factor, request.Factor)
                    .Set(e => e.DateLastUpdated, DateTime.Now);
                await _context.Entities.UpdateOneAsync(filter, update);

                return entity.Id;
            }
        }
        #endregion
    }
}