
using LifeStyle.Application.Planners.Commands;
using LifeStyle.Application.Planners.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlannerController : ControllerBase
    {

        private readonly IMediator _mediator;

        public PlannerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlanners()
        {
            var request = new GetAllPlanners();
            var result = await _mediator.Send(request);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePlanner([FromBody] CreatePlanner createPlanner)
        {
            var result = await _mediator.Send(createPlanner);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser([FromBody] DeletePlanner request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }


        [HttpPost("update")]
        public async Task<IActionResult> UpdatePlanner([FromBody] UpdatePlanner request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}