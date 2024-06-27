using AutoMapper;
using LifeStyle.Aplication.Logic;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Exercises.Responses;
using LifeStyle.Application.Meals.Query;
using LifeStyle.Application.Meals.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Application.Services;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Paged;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public MealController(IMediator mediator,IMapper mapper, IFileService fileService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<MealDto>>> GetAllMeals([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _mediator.Send(new GetAllMeals(pageNumber, pageSize));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving all meals: " + ex.Message);
            }
        }


        [HttpGet("{mealId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMealById(int mealId)
        {
            try
            {
                var request = new GetMealById(mealId);
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Meal not found: " + ex.Message);
            }
        }

        [HttpGet("filter")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<PagedResult<Meal>>> FilterMeals([FromQuery] MealFilterDto filterDto, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 6)
        {
            try
            {
                var query = new FilterMealQuery(filterDto, pageNumber, pageSize);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while filtering meals: " + ex.Message);
            }
        }


        [HttpGet("search")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<Meal>>> SearchMeals([FromQuery] string searchTerm)
        {
            try
            {
                var query = new SearchMealsQuery(searchTerm);
                var meals = await _mediator.Send(query);
                return Ok(meals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while search meals: " + ex.Message);
            }
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateMeal([FromForm] MealDto mealDto)
        {
            try
            {
                var createMealDto = _mapper.Map<MealDto>(mealDto);
                var nutrients = new Nutrients
                {
                    Calories = createMealDto.Nutrients.Calories,
                    Protein = createMealDto.Nutrients.Protein,
                    Carbohydrates = createMealDto.Nutrients.Carbohydrates,
                    Fat = createMealDto.Nutrients.Fat
                };

                var command = new CreateMeal(
                    createMealDto.MealName,
                    createMealDto.MealType,
                    nutrients,
                    createMealDto.Ingredients,
                    createMealDto.PreparationInstructions,
                    createMealDto.EstimatedPreparationTimeInMinutes,
                    createMealDto.Allergies,
                    createMealDto.Diets,
                    createMealDto.ImageUrl
                );

                var result = await _mediator.Send(command);

                return Ok( result);
            }

            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred while creating the meal: {ex.Message}");
            }
        }



        [HttpDelete("{mealId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMeal(int mealId)
        {
            try
            {
                var request = new DeleteMeal(mealId);
                await _mediator.Send(request);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound("Meal not found: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMeal(int id, [FromForm] MealDto mealDto, [FromForm] IFormFile image)
        {
            try
            {
                var updateMealDto = _mapper.Map<MealDto>(mealDto);
                var nutrients = new Nutrients
                {
                    Calories = updateMealDto.Nutrients.Calories,
                    Protein = updateMealDto.Nutrients.Protein,
                    Carbohydrates = updateMealDto.Nutrients.Carbohydrates,
                    Fat = updateMealDto.Nutrients.Fat
                };

                var command = new UpdateMeal(
                    id,
                    updateMealDto.MealName,
                    updateMealDto.MealType,
                    nutrients,
                    updateMealDto.Ingredients,
                    updateMealDto.PreparationInstructions,
                    updateMealDto.EstimatedPreparationTimeInMinutes,
                    image,
                    updateMealDto.Allergies,
                    updateMealDto.Diets
                );

                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred while updating the meal: {ex.Message}");
            }
        }


    }
}
