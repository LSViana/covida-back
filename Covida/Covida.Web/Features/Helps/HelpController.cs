using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Covida.Web.Features.Helps
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HelpController : ControllerBase
    {
        private readonly IMediator mediator;

        public HelpController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] List.Query query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Read([FromRoute] Guid id)
        {
            var response = await mediator.Send(new Read.Query { Id = id });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create.Command command)
        {
            var response = await mediator.Send(command);
            return Created($"{nameof(Read)}/${response.Id}", response);
        }

        [HttpGet("answer/{helpId}")]
        public async Task<IActionResult> Answer([FromRoute] Guid helpId)
        {
            var response = await mediator.Send(new Answer.Command { HelpId = helpId });
            return NoContent();
        }

        [HttpGet("mine")]
        public async Task<IActionResult> MyHelps()
        {
            var response = await mediator.Send(new Mine.Query());
            return Ok(response);
        }

        [HttpGet("update-help-item/{helpItemId}/{complete}")]
        public async Task<IActionResult> UpdateHelpItem([FromRoute] Guid helpItemId, [FromRoute] bool complete)
        {
            var response = await mediator.Send(new UpdateHelpItem.Command { HelpItemId = helpItemId, Complete = complete });
            return NoContent();
        }
    }
}