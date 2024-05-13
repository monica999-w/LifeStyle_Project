using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Serilog;
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
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("GetAllUsersHandler instance created.");
        }

        public async Task<List<UserDto>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAllUsers command...");
            try
            {
                var users = await _unitOfWork.UserProfileRepository.GetAll();
                return _mapper.Map<List<UserDto>>(users);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve all users");
                throw new Exception("Failed to retrieve all users", ex);
            }
        }
    }


}
