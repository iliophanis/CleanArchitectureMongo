using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Validators;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Entities.Queries
{
    public class ListEntities
    {
        #region Query
        public class Query : IRequest<Envelope<EntityDto>>
        {
            public PageParameters PageParameters { get; set; }
        }
        #endregion

        #region Validator
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(r => r.PageParameters.SortBy).NotEmpty();
                RuleFor(r => r.PageParameters).ValidPageParameters(typeof(Entity));
            }
        }
        #endregion

        #region Handler
        public class Handler : IRequestHandler<Query, Envelope<EntityDto>>
        {
            private readonly IContext _context;
            private readonly IMapper _mapper;

            public Handler(IContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Envelope<EntityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var envelope = await _context.GetEnvelopeAsync<Entity>(
                    _context.Entities,
                    request.PageParameters
                );

                return new Envelope<EntityDto>
                {
                    Items = _mapper.Map<List<EntityDto>>(envelope.Items),
                    Count = envelope.Count
                };
            }
        }
        #endregion
    }
}