using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Planners.Commands
{
    public record DeletePlanner (int PlannerId) : IRequest<Planner>;
    public class DeletePlannerHandler : IRequestHandler<DeletePlanner, Planner>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePlannerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Log.Information("DeletePlannerHandler instance created.");
        }

        public async Task<Planner> Handle(DeletePlanner request, CancellationToken cancellationToken)
        {
            Log.Information("Handling DeletePlanner command...");

            try
            {
                var planner = await _unitOfWork.PlannerRepository.GetPlannerById(request.PlannerId);
                if (planner == null)
                {
                    throw new NotFoundException($"Planner with ID {request.PlannerId} not found");
                }

                await _unitOfWork.PlannerRepository.RemovePlanner(planner);
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
                Log.Error(ex, "Failed to delete planner");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to delete planner", ex);
            }

        }
    }
}