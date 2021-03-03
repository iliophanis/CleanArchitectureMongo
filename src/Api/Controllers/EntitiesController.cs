using System.Threading.Tasks;
using Application.Common.Models;
using Application.Entities;
using Application.Entities.Commands;
using Application.Entities.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class EntitiesController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<string>> CreateEntity([FromBody] CreateEntity.Command request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet]
        public async Task<ActionResult<EntityDto>> ReadEntity([FromQuery] ReadEntity.Query request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateEntity([FromBody] UpdateEntity.Command request)
        {
            return await Mediator.Send(request);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteEntity([FromQuery] DeleteEntity.Command request)
        {
            await Mediator.Send(request);

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<Envelope<EntityDto>>> ListEntites([FromQuery] ListEntities.Query request)
        {
            return await Mediator.Send(request);
        }
    }
}