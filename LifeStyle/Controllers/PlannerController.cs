﻿using AutoMapper;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Planners.Commands;
using LifeStyle.Application.Planners.Query;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.Work.WebApi;



namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlannerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PlannerController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<PlannerDto>>> GetAllPlanners()
        {
            try
            {
                var request = new GetAllPlanners();
                var result = await _mediator.Send(request);
                var mappedResult = _mapper.Map<List<PlannerDto>>(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePlanner([FromBody] PlannerDto plannerDto)
        {
            try
            {
                var command = new CreatePlanner(
            plannerDto.ProfileId,
            plannerDto.MealIds,
            plannerDto.ExerciseIds
        );

                var result = await _mediator.Send(command);

                // Construim un nou obiect PlannerDto cu doar ID-urile
                var mappedResult = new PlannerDto
                {
                    PlannerId = result.PlannerId,
                    ProfileId = plannerDto.ProfileId,
                    MealIds = result.Meals?.Select(m => m.MealId).ToList() ?? new List<int>(),
                    ExerciseIds = result.Exercises?.Select(e => e.ExerciseId).ToList() ?? new List<int>()
                };

                return Ok(mappedResult);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }


        [HttpDelete("{plannerId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeletePlanner(int plannerId)
        {
            try
            {
                var command = new DeletePlanner(plannerId);
                var result = await _mediator.Send(command);
                if (result == null)
                    return NotFound();

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdatePlanner([FromBody] PlannerDto plannerDto)
        {
            try
            {
                var command = new UpdatePlanner(
                    plannerDto.PlannerId,
                    plannerDto.MealIds,
                    plannerDto.ExerciseIds
                );

                var result = await _mediator.Send(command);

                if (result)
                {
                    return Ok("Planner updated successfully");
                }

                return BadRequest("Failed to update planner");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DataValidationException ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
    }
}