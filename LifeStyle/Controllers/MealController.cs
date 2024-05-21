using AutoMapper;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Meals.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MealController(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealDto>>> GetAllMeals()
        {
            try
            {
                var request = new GetAllMeals();
                var result = await _mediator.Send(request);
                var mappedResult = _mapper.Map<List<MealDto>>(result);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving all meals: " + ex.Message);
            }
        }

        [HttpGet("{mealId}")]
        public async Task<IActionResult> GetMealById(int mealId)
        {
            try
            {
                var request = new GetMealById(mealId);
                var result = await _mediator.Send(request);
                var mappedResult = _mapper.Map<MealDto>(result);
                return Ok(mappedResult);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Meal not found: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeal([FromBody] MealDto? mealDto)
        {
            try
            {
                var command = new CreateMeal(
                    mealDto.Name,
                    mealDto.MealType,
                    mealDto.Nutrients
                );

                var result = await _mediator.Send(command);
                var mappedResult = _mapper.Map<MealDto>(result);

                return Ok(mappedResult);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
        }

         [HttpDelete("{mealId}")]
        public async Task<IActionResult> DeleteMeal(int mealId)
        {
            try
            {
                var request = new DeleteMeal(mealId);
                await _mediator.Send(request);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound("Meal not found: " + ex.Message);
            }
        }

        [HttpPut("{mealId}")]
        public async Task<IActionResult> UpdateMeal(int mealId, [FromBody] MealDto mealDto)
        {
            try
            {
                if (mealDto == null || mealId != mealDto.Id)
                {
                    return BadRequest("Invalid meal data or mealId in the path does not match mealId in the data.");
                }

                // Actualizăm meal-ul
                var command = new UpdateMeal(
                    mealId,
                    mealDto.Name,
                    mealDto.MealType,
                    _mapper.Map<Nutrients>(mealDto.Nutrients)
                );

                var updatedMeal = await _mediator.Send(command);

                if (updatedMeal != null)
                {
                    var mappedResult = _mapper.Map<MealDto>(updatedMeal);
                    return Ok(mappedResult);
                }
                else
                {
                    return NotFound($"Meal with ID {mealId} not found.");
                }
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update meal: {ex.Message}");
            }
        }
    }
}
