using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Serilog;

namespace LifeStyle.Application.Planners.Query
{
    public record GetPlannersByUser(UserProfile UserProfile) : IRequest<List<PlannerDto>>;
    public class GetPlannersByUserHandler : IRequestHandler<GetPlannersByUser, List<PlannerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPlannersByUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("GetPlannersByUserHandler instance created.");
           
        }

        public async Task<List<PlannerDto>> Handle(GetPlannersByUser request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetPlannersByUser command...");
            try
            {
                var planner = await _unitOfWork.PlannerRepository.GetPlannerByUser(request.UserProfile);
                if (planner == null)
                {
                    Log.Warning("No planner found for user {UserId}.", request.UserProfile.ProfileId);
                    return new List<PlannerDto>();
                }
                var plannerDto = _mapper.Map<PlannerDto>(planner);
                Log.Information("Planner found for user {UserId}.", request.UserProfile.ProfileId);
                return new List<PlannerDto> { plannerDto };
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
