using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record CreateNutrient(double Calories,  double Protein, double Carbohydrates , double Fat ) : IRequest<NutrientDto>;

    public class CreateNutrientHandler : IRequestHandler<CreateNutrient, NutrientDto>
    {

        private readonly IUnitOfWork _unitOfWork;


        public CreateNutrientHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NutrientDto> Handle(CreateNutrient request, CancellationToken cancellationToken)
        {

            try
            {
                var newNutrient = new Nutrients
                {
                        Calories = request.Calories,
                        Protein = request.Protein,
                        Carbohydrates = request.Carbohydrates,
                        Fat = request.Fat
                        
                };

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.NutrientRepository.Add(newNutrient);
           

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
                return NutrientDto.FromNutrient(newNutrient); 

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Failed to create nutrient", ex);
            }

        }
    }
}