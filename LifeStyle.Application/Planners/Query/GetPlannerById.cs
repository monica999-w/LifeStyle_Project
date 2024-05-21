using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Models.Planner;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Planners.Query
{
    public record GetPlannersById(int PlannerId) : IRequest<Planner>;
    public class GetPlannersByIdHandler : IRequestHandler<GetPlannersById, Planner>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetPlannersByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("GetPlannersByUserHandler instance created.");

        }

        public async Task<Planner> Handle(GetPlannersById request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetPlannersByUser command...");
            try
            {
                var planner = await _unitOfWork.PlannerRepository.GetPlannerById(request.PlannerId);
                if (planner == null)
                {
                    Log.Warning("No planner found", request.PlannerId);
                    throw new NotFoundException($"Planner not found");
                }
                return planner;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "An error occurred while handling GetPlannersByUser command.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve planner");
                throw new Exception("Failed to retrieve planner", ex);
            }
        }
    }

}
