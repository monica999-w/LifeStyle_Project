using Microsoft.AspNetCore.Mvc;
using MediatR;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Query;
using LifeStyle.Domain.Exception;
using AutoMapper;
using LifeStyle.Domain;
using System.ComponentModel.DataAnnotations;
using LifeStyle.Domain.Models.Exercises;
using static LifeStyle.Domain.InputValidator;
using LifeStyle.Application.Responses;



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
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExerciseDto exercise)
        {
            try
            {
                var command = new CreateExercise(exercise.Name, exercise.DurationInMinutes, exercise.Type);
                var result = await _mediator.Send(command);
              
                return Ok(result);
            }
            catch (AlreadyExistsException ex)
            {
                return Conflict("Exercise already exists: " + ex.Message);
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
        }

        [HttpPut("{exerciseId}")]
        public async Task<IActionResult> Update(int exerciseId, [FromBody]ExerciseDto updateExercise)
        {
            try
            {
                var command = new UpdateExercise(updateExercise.Id,updateExercise.Name, updateExercise.DurationInMinutes, updateExercise.Type);

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
        }

    }
}