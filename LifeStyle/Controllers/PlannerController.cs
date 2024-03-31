using LifeStyle.Interfaces;
using LifeStyle.Models.Planner;
using LifeStyle.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlannerController : Controller
    {

        private readonly IPlannerRepository _planner;

        public PlannerController(IPlannerRepository _planner)
        {
           this._planner = _planner;
        }

      

        [HttpGet("planner/{id}")]
        public async Task<ActionResult<Planner>> GetPlannerById(int id)
        {
            var userProfile = new UserProfile(id, "", "", 0, 0); 
            var planner = await _planner.GetPlannerByUser(userProfile);

            if (planner == null)
            {
                return NotFound();
            }

            return Ok(planner);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePlanner([FromBody] Planner planner)
        {
            await _planner.AddPlanner(planner);
            return Ok();
        }

        [HttpPut("planner/{id}")]
        public async Task<ActionResult> UpdatePlanner(int id, [FromBody] Planner planner)
        {
            if (id != planner.Profile.Id)
            {
                return BadRequest();
            }

            var existingPlanner = await _planner.GetPlannerByUser(planner.Profile);
            if (existingPlanner == null)
            {
                return NotFound();
            }

            await _planner.UpdatePlannerAsync(planner);
            return Ok();
        }

        [HttpDelete("planner/{id}")]
        public async Task<ActionResult> DeletePlanner(int id)
        {
            var userProfile = new UserProfile(id, "", "", 0, 0); 
            var planner = await _planner.GetPlannerByUser(userProfile);

            if (planner == null)
            {
                return NotFound();
            }

            await _planner.RemovePlanner(planner);
            return Ok();
        }
    }
}
