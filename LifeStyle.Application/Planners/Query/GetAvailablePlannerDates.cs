using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Planners.Query
{
    public record GetAvailablePlannerDates(string Email) : IRequest<IEnumerable<DateTime>>;

    public class GetAvailablePlannerDatesHandler : IRequestHandler<GetAvailablePlannerDates, IEnumerable<DateTime>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAvailablePlannerDatesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("GetAvailablePlannerDatesHandler instance created.");
        }

        public async Task<IEnumerable<DateTime>> Handle(GetAvailablePlannerDates request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAvailablePlannerDates command...");
            try
            {
                var user = await _unitOfWork.PlannerRepository.GetByEmail(request.Email);
                if (user == null)
                {
                    Log.Warning("User with email {Email} not found.", request.Email);
                    throw new NotFoundException($"User with email {request.Email} not found");
                }

                var dates = await _unitOfWork.PlannerRepository.GetAvailablePlannerDates(user.ProfileId);
                return dates;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve available planner dates");
                throw new Exception("Failed to retrieve available planner dates", ex);
            }
        }
    }

}
