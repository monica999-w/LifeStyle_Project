using LifeStyle.LifeStyle.Aplication.Interfaces;
using LifeStyle.LifeStyle.Domain.Models.Meal;
using Microsoft.AspNetCore.Mvc;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : Controller
    {
        private readonly IRepository<Meal> _meal;
       
        public MealController(IRepository<Meal> _meal)
        {
            this._meal = _meal;
        }

        [HttpGet]
        public async Task<IEnumerable<Meal>> GetAllMeal()
        {
            return await _meal.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> AddMeal(Meal meal)
        {
            await _meal.Add(meal);
            return Ok();
        }

        [HttpDelete("meal/{id}")]
        public async Task<IActionResult> DeleteMeal(int id, Meal meal)
        {
            if (id != meal.Id)
            {
                return BadRequest();
            }
            try
            {
                await _meal.Remove(meal);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("meal/{id}")]
        public async Task<IActionResult> UpdateMeal(int id, Meal updatedMeal)
        {
            if (id != updatedMeal.Id)
            {
                return BadRequest();
            }

            try
            {
                await _meal.Update(updatedMeal);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("meal/{id}")]
        public async Task<IActionResult> GetMealById(int id)
        {
            var meal = await _meal.GetById(id);
            if (meal == null)
            {
                return NotFound();
            }

            return Ok(meal);
        }

    }
}