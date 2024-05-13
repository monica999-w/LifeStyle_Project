using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Threading;


namespace LifeStyle.Application.Commands
{
    public record CreateMeal(string Name, MealType MealType, Nutrients Nutrients) : IRequest<MealDto>;

    public class CreateMealHandler : IRequestHandler<CreateMeal, MealDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRequestHandler<CreateNutrient, NutrientDto> _createNutrientHandler;

        public CreateMealHandler(IUnitOfWork unitOfWork,IMapper mapper, IRequestHandler<CreateNutrient, NutrientDto> createNutrientHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createNutrientHandler = createNutrientHandler;
            Log.Information("CreateMealHandler instance created.");
        }

        public async Task<MealDto> Handle(CreateMeal request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Handling CreateMeal command...");
                Nutrients nutrient;

                var existingNutrient = await _unitOfWork.NutrientRepository.GetById(request.Nutrients.NutrientId);
                if (existingNutrient != null)
                {
                    nutrient = existingNutrient;
                    Log.Information("Existing nutrient found: {NutrientId}", nutrient.NutrientId);
                }
                else
                {
                    var nutrientDto = await _createNutrientHandler.Handle(
                    new CreateNutrient(request.Nutrients.Calories, request.Nutrients.Protein, request.Nutrients.Carbohydrates, request.Nutrients.Fat),
                    cancellationToken);
                    nutrient= NutrientDto.FromNutrientDto(nutrientDto);
                    Log.Information("New nutrient created: {NutrientId}", nutrient.NutrientId);

                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    Log.Error("Meal name cannot be empty");
                    throw new ValidationException("Meal name cannot be empty");
                }
                var existingMeal = await _unitOfWork.MealRepository.GetByName(request.Name);

                if (existingMeal != null)
                {
                    Log.Error($"A meal with the name '{request.Name}' already exists");
                    throw new AlreadyExistsException($"A meal with the name '{request.Name}' already exists");
                } 
                await _unitOfWork.BeginTransactionAsync();

                var newMeal = new Meal
                {
                        Name = request.Name,
                        MealType = request.MealType,
                        Nutrients = nutrient
                };
               
               await _unitOfWork.MealRepository.Add(newMeal);
               await _unitOfWork.SaveAsync();
               await _unitOfWork.CommitTransactionAsync();
               Log.Information("Meal created successfully: {MealName}", newMeal.Name);

                return _mapper.Map<MealDto>(newMeal);
            }
            catch (ValidationException ex)
            {
                Log.Error(ex, "Validation error");
                throw;
            }
            catch (AlreadyExistsException ex)
            {
                Log.Error(ex, "Already exists error");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create meal");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to create meal", ex);
            }
        }
    }
}

