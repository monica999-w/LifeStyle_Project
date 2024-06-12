using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Responses;
using LifeStyle.Application.Users.Commands;
using LifeStyle.Application.Users.Query;
using LifeStyle.Domain.Exception;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
       [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                var request = new GetAllUsers();
                var result = await _mediator.Send(request);
                var mappedResult = _mapper.Map<List<UserDto>>(result);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{userId}")]
     //   [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> GetUserById(int userId)
        {
            try
            {
                var request = new GetUserById(userId);
                var result = await _mediator.Send(request);
                var mappedResult = _mapper.Map<UserDto>(result);
                return Ok(mappedResult);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
      //   [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<UserDto>(userDto);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var request = new DeleteUser(userId);
                await _mediator.Send(request);
                if (request == null)
                    return NotFound();
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
                var updatedUserDto = _mapper.Map<UserDto>(result);
                return Ok(updatedUserDto);
            }
            catch (DataValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
