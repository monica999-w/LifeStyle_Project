using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Users.Query
{
    public record GetUserById(int UserId) : IRequest<UserDto>;
    public class GetUserByIdHandler : IRequestHandler<GetUserById, UserDto>
    {
        private readonly IRepository<UserProfile> _userRepository;

        public GetUserByIdHandler(IRepository<UserProfile> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found");

            return UserDto.FromUser(user);
        }
    }

}
