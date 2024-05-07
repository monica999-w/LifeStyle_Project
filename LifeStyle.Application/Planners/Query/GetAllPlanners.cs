using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Domain.Exception;
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
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPlannersHandler( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PlannerDto>> Handle(GetAllPlanners request, CancellationToken cancellationToken)
        {
            try
            {
                var planners = await _unitOfWork.PlannerRepository.GetAll();
                return planners.Select(planner => PlannerDto.FromPlanner(planner)).ToList();
            }
            catch (Exception ex)
            {
                throw new DataValidationException("Failed to retrieve all planners", ex);
            }
        }
    }

}
