using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Covida.Web.Features.HelpCategories
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HelpCategoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public HelpCategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] List.Query query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }
    }
}