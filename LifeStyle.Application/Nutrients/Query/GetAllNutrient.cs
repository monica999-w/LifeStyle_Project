using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Query
{
    public record GetAllNutrient : IRequest<List<NutrientDto>>;
    public class GetAllNutrientsHandler : IRequestHandler<GetAllNutrient, List<NutrientDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllNutrientsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<NutrientDto>> Handle(GetAllNutrient request, CancellationToken cancellationToken)
        {
            var exercise = await _unitOfWork.NutrientRepository.GetAll();
            return exercise.Select(NutrientDto.FromNutrient).ToList();
        }
    }

}
