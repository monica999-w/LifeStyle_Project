using LifeStyle.Interfaces;
using LifeStyle.Models.Exercises;
using LifeStyle.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : Controller
    {
        private readonly IRepository<Exercise> _exercise;

        public ExerciseController(IRepository<Exercise> _exercise)
        {
            this._exercise = _exercise;
        }

        [HttpGet]
        public async Task<IEnumerable<Exercise>> GetAllExercise()
        {
            return await _exercise.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Exercise exercise)
        {
            await _exercise.Add(exercise);
            return Ok();
        }

        [HttpDelete("exercise/{id}")]
        public async Task<IActionResult> DeleteUser(int id, Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return BadRequest();
            }
            try
            {
                await _exercise.Remove(exercise);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("exercise/{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, Exercise updatedExercise)
        {
            if (id != updatedExercise.Id)
            {
                return BadRequest();
            }

            try
            {
                await _exercise.Update(updatedExercise);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("exercise/{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            var exercise = await _exercise.GetById(id);
            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

    }
}