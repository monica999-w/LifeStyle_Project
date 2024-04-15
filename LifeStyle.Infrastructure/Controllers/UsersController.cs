using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Users.Commands;
using LifeStyle.Application.Users.Query;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var request = new GetAllUsers();
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var request = new GetUserById(userId);
            var result = await _mediator.Send(request);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var request = new DeleteUser(userId);
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId,UpdateUser request)
        {
            if (userId != request.UserId)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}