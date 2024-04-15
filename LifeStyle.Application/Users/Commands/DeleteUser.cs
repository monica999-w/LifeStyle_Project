using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;



namespace LifeStyle.Application.Users.Commands
{
    public record DeleteUser(int UserId) : IRequest<Unit>;

    public class DeleteUserHandler : IRequestHandler<DeleteUser, Unit>
    {

        private readonly IRepository<UserProfile> _userRepository;


        public DeleteUserHandler(IRepository<UserProfile> mealRepository)
        {
            _userRepository = mealRepository;
        }

        public async Task<Unit> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("Exercise not found");
            }

            await _userRepository.Remove(user);
            return Unit.Value;
        }
    }
}

