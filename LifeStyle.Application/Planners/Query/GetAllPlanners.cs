using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Planners.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Planners.Query
{
    public record GetAllPlanners : IRequest<List<PlannerDto>>;
    public class GetAllPlannersHandler : IRequestHandler<GetAllPlanners, List<PlannerDto>>
    {
        private readonly IPlannerRepository _plannerRepository;

        public GetAllPlannersHandler(IPlannerRepository plannerRepository)
        {
            _plannerRepository = plannerRepository;
        }

        public async Task<List<PlannerDto>> Handle(GetAllPlanners request, CancellationToken cancellationToken)
        {
            var planners = await _plannerRepository.GetAll();
            return planners.Select(planner => PlannerDto.FromPlanner(planner)).ToList();
        }
    }

}
