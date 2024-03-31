using LifeStyle.Interfaces;
using LifeStyle.Logic;
using LifeStyle.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IRepository<UserProfile> _user;

        public UsersController(IRepository<UserProfile> _user)
        {
            this._user = _user;
        }

        [HttpGet]
        public async Task<IEnumerable<UserProfile>> GetAllUsers()
        {
            return await _user.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserProfile profile)
        {
            await _user.Add(profile);
            return Ok();
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(int id,UserProfile profile)
        {
            if (id != profile.Id)
            {
                return BadRequest();
            }
            try
            {
                await _user.Remove(profile);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, UserProfile updatedProfile)
        {
            if (id != updatedProfile.Id)
            {
                return BadRequest();
            }

            try
            {
                await _user.Update(updatedProfile);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            var userProfile = await _user.GetById(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

    }
}
         
      