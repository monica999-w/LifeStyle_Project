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
    public record GetPlannerByEmailAndDate(string Email, DateTime Date) : IRequest<Planner?>;

    public class GetPlannerByEmailAndDateHandler : IRequestHandler<GetPlannerByEmailAndDate, Planner?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlannerByEmailAndDateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("GetPlannerByEmailAndDateHandler instance created.");
        }

        public async Task<Planner?> Handle(GetPlannerByEmailAndDate request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetPlannerByEmailAndDate command...");
            try
            {
                var user = await _unitOfWork.PlannerRepository.GetByEmail(request.Email);
                if (user == null)
                {
                    Log.Warning("User with email {Email} not found.", request.Email);
                    throw new NotFoundException($"User with email {request.Email} not found");
                }

                var planner = await _unitOfWork.PlannerRepository.GetPlannerByDate(user.ProfileId, request.Date.Date);
                return planner;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "An error occurred while handling GetPlannerByEmailAndDate command.");
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