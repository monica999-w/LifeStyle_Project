using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Meals.Query;
using LifeStyle.Application.Query;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;


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
            var request = new GetAllMeals();
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{mealId}")]
        public async Task<IActionResult> GetMealById(int mealId)
        {
            var request = new GetMealById(mealId);
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeal(CreateMeal request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{mealId}")]
        public async Task<IActionResult> DeleteMeal(int mealId)
        {
            var request = new DeleteMeal(mealId);
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpPut("{mealId}")]
        public async Task<IActionResult> UpdateMeal(int mealId, [FromBody] UpdateMeal request)
        {
            if (mealId != request.MealId)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}