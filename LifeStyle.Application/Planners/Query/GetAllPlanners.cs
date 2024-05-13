using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using MediatR;
using Serilog;
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
        private readonly IMapper _mapper;

        public GetAllPlannersHandler( IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            Log.Information("GetAllPlannersHandler instance created.");
           
        }

        public async Task<List<PlannerDto>> Handle(GetAllPlanners request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAllPlanners command...");
            try
            {
                var planners = await _unitOfWork.PlannerRepository.GetAll();
                return _mapper.Map<List<PlannerDto>>(planners);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve all exercises");
                throw new Exception("Failed to retrieve all planners", ex);
            }
        }
    }

}
