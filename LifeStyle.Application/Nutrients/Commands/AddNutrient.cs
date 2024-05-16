using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record CreateNutrient(double Calories,  double Protein, double Carbohydrates , double Fat ) : IRequest<NutrientDto>;

    public class CreateNutrientHandler : IRequestHandler<CreateNutrient, NutrientDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CreateNutrientHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("CreateNutrientHandler instance created.");
        }

        public async Task<NutrientDto> Handle(CreateNutrient request, CancellationToken cancellationToken)
        {
            Log.Information("Handling CreateNutrient command...");
            try
            {
                var newNutrient = new Nutrients
                {
                        Calories = request.Calories,
                        Protein = request.Protein,
                        Carbohydrates = request.Carbohydrates,
                        Fat = request.Fat
                        
                };

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                Log.Information("Adding newNutrient to repository...");
                await _unitOfWork.NutrientRepository.Add(newNutrient);
           

                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                var nutrientDto = _mapper.Map<NutrientDto>(newNutrient);

                return nutrientDto;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create nutrient");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to create nutrient", ex);
            }

        }
    }
}