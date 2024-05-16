using LifeStyle.Application.Planners.Commands;
using LifeStyle.Application.Planners.Query;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Domain.Exception;
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
            try
            {
                var request = new GetAllPlanners();
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlanner([FromBody] PlannerDto planner)
        {
            try
            {
              
                var result = await _mediator.Send(planner);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DataValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

       

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser([FromBody] DeletePlanner request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdatePlanner([FromBody] UpdatePlanner request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DataValidationException ex)
            {
                return BadRequest(ex.Message);
            }  
        }
    }
}