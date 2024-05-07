using LifeStyle.Application.Commands;
using LifeStyle.Application.Meals.Query;
using LifeStyle.Domain.Exception;
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

        public MealController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMeals()
        {
            try
            {
                var request = new GetAllMeals();
                var result = await _mediator.Send(request);
                return Ok(result);
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
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Meal not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving the meal: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeal(CreateMeal request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest("Invalid request: " + ex.Message);
            }
            catch (AlreadyExistsException ex)
            {
                return Conflict("Meal already exists: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while creating the meal: " + ex.Message);
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
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while deleting the meal: " + ex.Message);
            }
        }

        [HttpPut("{mealId}")]
        public async Task<IActionResult> UpdateMeal(int mealId, [FromBody] UpdateMeal request)
        {
            if (mealId != request.MealId)
            {
                return BadRequest("Mismatch between meal ID in URL and request body");
            }

            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Meal not found: " + ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest("Invalid request: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while updating the meal: " + ex.Message);
            }
        }
    }
}
