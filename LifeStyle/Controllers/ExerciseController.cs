using Microsoft.AspNetCore.Mvc;
using MediatR;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Users.Query;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Query;
using System.ComponentModel.DataAnnotations;
using LifeStyle.Domain.Exception;



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
            try
            {
                var request = new GetAllExercise();
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (DataValidationException ex)
            {
                return StatusCode(500, "An error occurred while retrieving all exercises from repository: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving all exercises: " + ex.Message);
            }
        }

        [HttpGet("{exerciseId}")]
        public async Task<IActionResult> GetExerciseById(int exerciseId)
        {
            try
            {
                var request = new GetExerciseById(exerciseId);
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Exercise not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving the exercise by ID: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateExercise command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (AlreadyExistsException ex)
            {
                return Conflict("Exercise already exists: " + ex.Message);
            }
            catch (DataValidationException ex)
            {
                return BadRequest("Failed to create exercise: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while creating the exercise: " + ex.Message);
            }
        }


        [HttpDelete("{exerciseId}")]
        public async Task<IActionResult> Delete(int exerciseId)
        {
            try
            {
                var result = await _mediator.Send(new DeleteExercise(exerciseId));
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound("Exercise not found: " + ex.Message);
            }
            catch (DataValidationException ex)
            {
                return StatusCode(500, "An error occurred while deleting the exercise: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while deleting the exercise: " + ex.Message);
            }
        }

        [HttpPut("{exerciseId}")]
        public async Task<IActionResult> Update(int exerciseId, UpdateExercise command)
        {
            try
            {
                if (exerciseId != command.ExerciseId)
                {
                    return BadRequest("Exercise ID in URL does not match Exercise ID in request body");
                }

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Exercise not found: " + ex.Message);
            }
            catch (DataValidationException ex)
            {
                return StatusCode(500, "An error occurred while updating the exercise: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while updating the exercise: " + ex.Message);
            }
        }

    }
}