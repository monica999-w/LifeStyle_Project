using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Users.Query
{
    public record GetUserById(int UserId) : IRequest<UserProfile>;
    public class GetUserByIdHandler : IRequestHandler<GetUserById, UserProfile>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("GetUserByIdHandler instance created.");
        }

        public async Task<UserProfile> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetUserById command for User ID {UserId}...", request.UserId);
            try
            {
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                {
                    Log.Warning("User not found: ID={UserId}", request.UserId);
                    throw new NotFoundException($"User with ID {request.UserId} not found");
                }
                return user;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "User not found");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve user");
                throw new Exception("Failed to retrieve user", ex);
            }
        }
    }

}
