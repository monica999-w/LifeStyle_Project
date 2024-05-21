using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record UpdateNutrient(int MealId,double Calories, double Protein, double Carbohydrates, double Fat) : IRequest<Nutrients>;

    public class UpdateNutrientHandler : IRequestHandler<UpdateNutrient, Nutrients>
    {

        private readonly IUnitOfWork _unitOfWork;

        public UpdateNutrientHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Log.Information("UpdateNutrientHandler instance created.");
        }

        public async Task<Nutrients> Handle(UpdateNutrient request, CancellationToken cancellationToken)
        {
            Log.Information("Handling UpdateNutrient command...");

            try
            {
                Log.Information("Updating nutrient with Id {NutrientId}", request.MealId);
                var nutrient = await _unitOfWork.NutrientRepository.GetById(request.MealId);
                

                nutrient.Protein=request.Protein;
                nutrient.Calories=request.Calories;
                nutrient.Carbohydrates=request.Carbohydrates;
                nutrient.Fat=request.Fat;

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.NutrientRepository.Update(nutrient);

                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();

                Log.Information("Nutrient updated successfully");

                return nutrient;
            }
            catch(NotFoundException ex)
            {
                Log.Error(ex, "Nutriebt not found");
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update nutrient");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Fail update exercise", ex);
            }

        }
    }
}


