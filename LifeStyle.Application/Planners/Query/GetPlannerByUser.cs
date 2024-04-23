using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Domain.Models.Users;
using MediatR;

namespace LifeStyle.Application.Planners.Query
{
    public record GetPlannersByUser(UserProfile UserProfile) : IRequest<List<PlannerDto>>;
    public class GetPlannersByUserHandler : IRequestHandler<GetPlannersByUser, List<PlannerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlannersByUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PlannerDto>> Handle(GetPlannersByUser request, CancellationToken cancellationToken)
        {
            var planner = await _unitOfWork.PlannerRepository.GetPlannerByUser(request.UserProfile);
            if (planner == null)
                return new List<PlannerDto>();

            return new List<PlannerDto> { PlannerDto.FromPlanner(planner) };
        }
    }


}
