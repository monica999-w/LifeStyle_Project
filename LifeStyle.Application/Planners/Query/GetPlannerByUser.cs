using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;
using MediatR;
using Serilog;

namespace LifeStyle.Application.Planners.Query
{
    public record GetPlannersByUser(UserProfile UserProfile) : IRequest<Planner>;
    public class GetPlannersByUserHandler : IRequestHandler<GetPlannersByUser,Planner>
    {
        private readonly IUnitOfWork _unitOfWork;
      

        public GetPlannersByUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("GetPlannersByUserHandler instance created.");
           
        }

        public async Task<Planner> Handle(GetPlannersByUser request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetPlannersByUser command...");
            try
            {
                var planner = await _unitOfWork.PlannerRepository.GetPlannerByUser(request.UserProfile);
                if (planner == null)
                {
                    Log.Warning("No planner found for user {UserId}.", request.UserProfile.ProfileId);
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
