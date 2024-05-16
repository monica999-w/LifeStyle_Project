using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Responses;
using LifeStyle.Application.Users.Commands;
using LifeStyle.Application.Users.Query;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static LifeStyle.Domain.InputValidator;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly EntityValidator<UserProfile> _validator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
            _validator = new EntityValidator<UserProfile>();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var request = new GetAllUsers();
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var request = new GetUserById(userId);
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            try
            {
                var command = new CreateUser(user.Email, user.PhoneNumber, user.Weight, user.Height);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            
            catch(AlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var request = new DeleteUser(userId);
                await _mediator.Send(request);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody]UserDto updateUser)
        {
            try
            { 
                
                var command = new UpdateUser(updateUser.Id,updateUser.Email,updateUser.PhoneNumber,updateUser.Weight, updateUser.Height);

                if (userId != updateUser.Id)
                {
                    return BadRequest("User ID in URL does not match User ID in request body");
                }
               

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (DataValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
