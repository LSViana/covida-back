using Covida.Data.Postgre;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covida.Web.Features.Authentication
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthenticationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create.Command command)
        {
            var response = await mediator.Send(command);
            return Created("me", response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login.Command command)
        {
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpPatch("edit-address")]
        public async Task<IActionResult> EditLocation([FromBody] EditAddress.Command command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
