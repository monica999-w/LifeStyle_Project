using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;


namespace LifeStyle.Application.Commands
{
    public record CreateMeal(string Name, MealType MealType, Nutrients Nutrients) : IRequest<MealDto>;

    public class CreateMealHandler : IRequestHandler<CreateMeal, MealDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestHandler<CreateNutrient, NutrientDto> _createNutrientHandler;

        public CreateMealHandler(IUnitOfWork unitOfWork, IRequestHandler<CreateNutrient, NutrientDto> createNutrientHandler)
        {
            _unitOfWork = unitOfWork;
            _createNutrientHandler = createNutrientHandler;
        }

        public async Task<MealDto> Handle(CreateMeal request, CancellationToken cancellationToken)
        {
            try
            {
                Nutrients nutrient;

                var existingNutrient = await _unitOfWork.NutrientRepository.GetById(request.Nutrients.NutrientId);
                if (existingNutrient != null)
                {
                    nutrient = existingNutrient;
                }
                else
                {
                    var nutrientDto = await _createNutrientHandler.Handle(
                    new CreateNutrient(request.Nutrients.Calories, request.Nutrients.Protein, request.Nutrients.Carbohydrates, request.Nutrients.Fat),
                    cancellationToken);
                    nutrient= NutrientDto.FromNutrientDto(nutrientDto);

                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    throw new ValidationException("Meal name cannot be empty");
                }
                var existingMeal = await _unitOfWork.MealRepository.GetByName(request.Name);

                if (existingMeal != null)
                {
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


               return MealDto.FromMeal(newMeal);
            }
            catch (ValidationException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
            catch (AlreadyExistsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Failed to create meal", ex);
            }
        }
    }
}

