using Microsoft.AspNetCore.Mvc;
using MediatR;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Users.Query;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Query;



namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExerciseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExercises()
        {
            var request = new GetAllExercise();
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{exerciseId}")]
        public async Task<IActionResult> GetExerciseById(int exerciseId)
        {
            var request = new GetExerciseById(exerciseId);
            var result = await _mediator.Send(request);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateExercise command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpDelete("{exerciseId}")]
        public async Task<IActionResult> Delete(int exerciseId)
        {
            var result = await _mediator.Send(new DeleteExercise(exerciseId));
            return Ok(result);
        }

        [HttpPut("{exerciseId}")]
        public async Task<IActionResult> Update(int exerciseId, UpdateExercise command)
        {
            if (exerciseId != command.ExerciseId)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}