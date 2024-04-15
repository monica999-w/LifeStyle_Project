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
    public record GetAllUsers : IRequest<List<UserDto>>;
    public class GetAllUsersHandler : IRequestHandler<GetAllUsers, List<UserDto>>
    {
        private readonly IRepository<UserProfile> _userRepository;

        public GetAllUsersHandler(IRepository<UserProfile> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAll(); 
            return users.Select(UserDto.FromUser).ToList();
        }
    }


}
