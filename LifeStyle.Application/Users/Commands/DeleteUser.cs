using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Serilog;



namespace LifeStyle.Application.Users.Commands
{
    public record DeleteUser(int UserId) : IRequest<Unit>;

    public class DeleteUserHandler : IRequestHandler<DeleteUser, Unit>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public DeleteUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("DeleteUserHandler instance created.");
        }

        public async Task<Unit> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            Log.Information("Handling DeleteUser command...");


            try
            {
                Log.Information("Deleting user with Id {UserId}", request.UserId);
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);

                if (user == null)
                {
                    Log.Warning("User not found: ID={UserId}", request.UserId);
                    throw new NotFoundException("User not found");
                }

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.UserProfileRepository.Remove(user);

                await _unitOfWork.SaveAsync();

                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("User deleted successfully: ID={UserId}", request.UserId);
                return Unit.Value;
            }
            catch(NotFoundException ex)
            {
                Log.Error(ex, "User not found");
                throw ;
            }

            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete user");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to delete user", ex);
            }
        }
    }
}

