using Microsoft.AspNetCore.Mvc;
using MediatR;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Query;
using LifeStyle.Domain.Exception;
using LifeStyle.Application.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;



namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;


        public ExerciseController(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAllExercises()
        {
            try 
            { 
                var result = await _mediator.Send( new GetAllExercise());
                var mappedResult = _mapper.Map<List<ExerciseDto>>(result);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving all exercises: " + ex.Message);
            }
        }

        [HttpGet("{exerciseId}")]
        public async Task<ActionResult> GetExerciseById(int exerciseId)
        {
            try
            {
                var exercise = await _mediator.Send(new GetExerciseById(exerciseId));
                if (exercise == null)
                    return NotFound();

                var mappedResult = _mapper.Map<ExerciseDto>(exercise);
                return Ok(mappedResult);

            }
            catch (NotFoundException ex)
            {
                return NotFound("Exercise not found: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExerciseDto exerciseDto)
        {
            try
            {
                var exercise = _mapper.Map<ExerciseDto>(exerciseDto);
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
                if (result == null)
                    return NotFound();

                return NoContent();
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

                var updatedExercise = await _mediator.Send(command);
                var updatedExerciseDto = _mapper.Map<ExerciseDto>(updatedExercise);
                return Ok(updatedExerciseDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Exercise not found: " + ex.Message);
            }
        }

    }
}