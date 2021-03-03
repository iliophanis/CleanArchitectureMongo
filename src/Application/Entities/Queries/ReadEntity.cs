using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Validators;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace Application.Entities.Queries
{
    public class ReadEntity
    {
        #region Query
        public class Query : IRequest<EntityDto>
        {
            public string Id { get; set; }
        }
        #endregion

        #region Validator
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(r => r.Id).ValidObjectId();
            }
        }
        #endregion

        #region Handler
        public class Handler : IRequestHandler<Query, EntityDto>
        {
            private readonly IContext _context;
            private readonly IMapper _mapper;

            public Handler(IContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<EntityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var filter = Builders<Entity>.Filter.Eq(e => e.Id, request.Id);

                var entity = (await _context.Entities.FindAsync(filter)).FirstOrDefault();
                if (entity == null)
                    throw new NotFoundException(typeof(Entity).ToString(), request.Id);

                return _mapper.Map<EntityDto>(entity);
            }
        }
        #endregion
    }
}