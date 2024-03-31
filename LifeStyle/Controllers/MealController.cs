using LifeStyle.Interfaces;
using LifeStyle.Models.Meal;
using LifeStyle.Models.User;
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
        public async Task<IEnumerable<Meal>> GetAllUsers()
        {
            return await _meal.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Meal meal)
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
        public async Task<IActionResult> UpdateUserProfile(int id, Meal updatedMeal)
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