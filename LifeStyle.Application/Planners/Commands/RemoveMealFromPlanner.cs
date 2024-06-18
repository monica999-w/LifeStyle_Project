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

namespace LifeStyle.Application.Planners.Commands
{
    public record RemoveMealFromPlanner(int PlannerId, int MealId) : IRequest<Planner>;

    public class RemoveMealFromPlannerHandler : IRequestHandler<RemoveMealFromPlanner, Planner>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveMealFromPlannerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("RemoveMealFromPlannerHandler instance created.");
        }

        public async Task<Planner> Handle(RemoveMealFromPlanner request, CancellationToken cancellationToken)
        {
            Log.Information("Handling RemoveMealFromPlanner command...");

            try
            {
                var planner = await _unitOfWork.PlannerRepository.GetPlannerById(request.PlannerId);
                if (planner == null)
                {
                    throw new NotFoundException($"Planner with ID {request.PlannerId} not found");
                }

                var meal = planner.Meals.FirstOrDefault(m => m.MealId == request.MealId);
                if (meal == null)
                {
                    throw new NotFoundException($"Meal with ID {request.MealId} not found in planner");
                }

                planner.Meals.Remove(meal);
                await _unitOfWork.SaveAsync();

                return planner;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Not found exception");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to remove meal from planner");
                throw new Exception("Failed to remove meal from planner", ex);
            }
        }
    }

}
