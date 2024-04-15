using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LifeStyle.Application.Responses.UserDto;

namespace LifeStyle.Application.Planners.Query
{
    public record GetPlannersByUser(UserProfile UserProfile) : IRequest<List<PlannerDto>>;
    public class GetPlannersByUserHandler : IRequestHandler<GetPlannersByUser, List<PlannerDto>>
    {
        private readonly IPlannerRepository _plannerRepository;

        public GetPlannersByUserHandler(IPlannerRepository plannerRepository)
        {
            _plannerRepository = plannerRepository;
        }

        public async Task<List<PlannerDto>> Handle(GetPlannersByUser request, CancellationToken cancellationToken)
        {
            var planner = await _plannerRepository.GetPlannerByUser(request.UserProfile);
            if (planner == null)
                return new List<PlannerDto>();

            return new List<PlannerDto> { PlannerDto.FromPlanner(planner) };
        }
    }


}
