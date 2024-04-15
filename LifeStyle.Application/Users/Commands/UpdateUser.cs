using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Users.Commands
{

    public record UpdateUser(int UserId,string Email, string PhoneNumber, double Height, double Weight) : IRequest<UserDto>;

    public class UpdateUserHandler : IRequestHandler<UpdateUser, UserDto>
    {

        private readonly IRepository<UserProfile> _userRepository;

        public UpdateUserHandler(IRepository<UserProfile> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found");
            }

            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Height = request.Height;
            user.Weight = request.Weight;

            await _userRepository.Update(user);
            return UserDto.FromUser(user);
        }
    }
}
