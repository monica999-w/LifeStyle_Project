using LifeStyle.Application.Commands;
using LifeStyle.Application.Meals.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Domain;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static LifeStyle.Domain.InputValidator;


namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly EntityValidator<Meal> _validator;

        public MealController(IMediator mediator)
        {
            _mediator = mediator;
            _validator = new EntityValidator<Meal>();
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
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeal([FromBody] MealDto meal)
        {
            try
            {
                var command = new CreateMeal(meal.Name, meal.MealType,meal.Nutrients);

                var result = await _mediator.Send(command);
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
        public async Task<IActionResult> UpdateMeal(int mealId, [FromBody] MealDto updateMeal)
        {
         
            try
            {
                var command = new UpdateMeal(mealId, updateMeal.Name, updateMeal.MealType, updateMeal.Nutrients);
                var result = await _mediator.Send(command);
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
        }
    }
}
