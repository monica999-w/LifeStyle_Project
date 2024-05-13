using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Users.Commands
{

    public record UpdateUser(int UserId,string Email, string PhoneNumber, double Height, double Weight) : IRequest<UserDto>;

    public class UpdateUserHandler : IRequestHandler<UpdateUser, UserDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("UpdateUserHandler instance created.");
           
        }

        public async Task<UserDto> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            Log.Information("Handling UpdateUser command...");
            try
            {
                Log.Information("Updating user with Id {UserId}", request.UserId);
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                {
                    Log.Warning("User not found: ID={UserId}", request.UserId);
                    throw new NotFoundException($"User with ID {request.UserId} not found");
                }

                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
                user.Height = request.Height;
                user.Weight = request.Weight;

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.UserProfileRepository.Update(user);

                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("User updated successfully: ID={UserId}", request.UserId);

                return _mapper.Map<UserDto>(user);
            }
            catch(NotFoundException ex)
            {
                Log.Error(ex, "User not found");
                throw ;
            }

            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update user");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Fail update user", ex);
            }
        }
    }
}
