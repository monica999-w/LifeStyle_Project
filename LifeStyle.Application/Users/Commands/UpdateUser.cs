using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using MediatR;


namespace LifeStyle.Application.Users.Commands
{

    public record UpdateUser(int UserId,string Email, string PhoneNumber, double Height, double Weight) : IRequest<UserDto>;

    public class UpdateUserHandler : IRequestHandler<UpdateUser, UserDto>
    {

        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> Handle(UpdateUser request, CancellationToken cancellationToken)
        {

            try
            {
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {request.UserId} not found");
                }

                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
                user.Height = request.Height;
                user.Weight = request.Weight;


                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.UserProfileRepository.Update(user);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                return UserDto.FromUser(user);
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Fail update user", ex);
            }
        }
    }
}
