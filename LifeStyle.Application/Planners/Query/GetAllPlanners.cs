using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
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
    public record GetAllPlanners : IRequest<List<Planner>>;
    public class GetAllPlannersHandler : IRequestHandler<GetAllPlanners, List<Planner>>
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public GetAllPlannersHandler( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            

            Log.Information("GetAllPlannersHandler instance created.");
           
        }

        public async Task<List<Planner>> Handle(GetAllPlanners request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAllPlanners command...");
            try
            {
                var planners = await _unitOfWork.PlannerRepository.GetAll();
                return planners.ToList();
            }

            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve all exercises");
                throw new Exception("Failed to retrieve all planners", ex);
            }
        }
    }

}
