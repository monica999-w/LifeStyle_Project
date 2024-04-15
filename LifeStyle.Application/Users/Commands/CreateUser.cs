using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Users;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record CreateUser( string Email ,string PhoneNumber, double Height, double Weight): IRequest<UserDto>;
    public  class CreateUserHander(IRepository<UserProfile> userRepository) : IRequestHandler<CreateUser,UserDto>
    {
        private readonly IRepository<UserProfile> _userRepository = userRepository;

        public async Task<UserDto> Handle(CreateUser request, CancellationToken cancellationToken)
        {

            var user = new UserProfile
            (
               id: GetNextId(),
               email: request.Email,
               phoneNumber: request.PhoneNumber,
               height: request.Height,
               weight: request.Weight
            );

            await _userRepository.Add(user);
            return UserDto.FromUser(user);
        }
        private int GetNextId()
        {
            var lastId = _userRepository.GetLastId();
            return lastId + 1;
        }
    }
}
