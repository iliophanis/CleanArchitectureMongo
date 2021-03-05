using System.Threading.Tasks;
using Application.Users.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsersController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<string>> Register(Register.Command request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(Login.Query request)
        {
            return await Mediator.Send(request);
        }
    }
}