using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Entities.Commands
{
    public class CreateEntity
    {
        #region Command
        public class Command : IRequest<string>
        {
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
            private readonly ICurrentUserService _currentUserService;

            public Handler(IContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = new Entity
                {
                    CreatedBy = _currentUserService.UserId,
                    DateCreated = DateTime.Now,
                    LastUpdatedBy = _currentUserService.UserId,
                    DateLastUpdated = DateTime.Now,
                    Name = request.Name,
                    Description = request.Description,
                    Age = request.Age,
                    Factor = request.Factor
                };

                await _context.Entities.InsertOneAsync(entity);

                return entity.Id;
            }
        }
        #endregion
    }
}