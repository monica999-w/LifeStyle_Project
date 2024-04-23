using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Commands
{
    public record UpdateNutrient(int NutrientId,double Calories, double Protein, double Carbohydrates, double Fat) : IRequest<NutrientDto>;

    public class UpdateNutrientHandler : IRequestHandler<UpdateNutrient, NutrientDto>
    {

        private readonly IUnitOfWork _unitOfWork;

        public UpdateNutrientHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NutrientDto> Handle(UpdateNutrient request, CancellationToken cancellationToken)
        {

            try
            {
                var nutrient = await _unitOfWork.NutrientRepository.GetById(request.NutrientId);
                if (nutrient == null)
                {
                    throw new Exception($"Nutrient with ID {request.NutrientId} not found");
                }

                nutrient.Protein=request.Protein;
                nutrient.Calories=request.Calories;
                nutrient.Carbohydrates=request.Carbohydrates;
                nutrient.Fat=request.Fat;
                

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.NutrientRepository.Update(nutrient);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                return NutrientDto.FromNutrient(nutrient);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Fail update exercise", ex);
            }

        }
    }
}


