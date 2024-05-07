using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record CreateUser( string Email ,string PhoneNumber, double Height, double Weight): IRequest<UserDto>;
    public  class CreateUserHander : IRequestHandler<CreateUser,UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserHander(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _unitOfWork.UserProfileRepository.GetByName(request.Email);
                if (existingUser != null)
                {
                    throw new AlreadyExistsException("User already exists");
                }
                
                await _unitOfWork.BeginTransactionAsync();

                var newUser = new UserProfile
                {
                    Email=request.Email,
                    PhoneNumber=request.PhoneNumber,
                    Height=request.Height,
                    Weight=request.Weight

                };

                await _unitOfWork.UserProfileRepository.Add(newUser);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();


                return UserDto.FromUser(newUser);
            }
            catch(AlreadyExistsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Failed to create user", ex);
            }
        }
    }
}
