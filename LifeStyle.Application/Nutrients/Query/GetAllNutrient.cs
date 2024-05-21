using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Query
{
    public record GetAllNutrient : IRequest<List<Nutrients>>;
    public class GetAllNutrientsHandler : IRequestHandler<GetAllNutrient, List<Nutrients>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllNutrientsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("GetAllNutrientsHandler instance created.");
        }

        public async Task<List<Nutrients>> Handle(GetAllNutrient request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAllNutrient command...");
            try
            {
                var nutrient = await _unitOfWork.NutrientRepository.GetAll();
                return nutrient;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve all nutriets");
                throw new Exception("Failed to retrieve all nutrients", ex);
            }
        }
    }

}
