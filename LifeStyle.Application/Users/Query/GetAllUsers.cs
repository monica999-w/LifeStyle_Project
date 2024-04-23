using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
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
        private readonly IUnitOfWork _unitOfWork;

        public GetAllUsersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDto>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserProfileRepository.GetAll(); 
            return users.Select(UserDto.FromUser).ToList();
        }
    }


}
