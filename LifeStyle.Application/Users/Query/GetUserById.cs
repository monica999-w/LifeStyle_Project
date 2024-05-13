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
    public record GetUserById(int UserId) : IRequest<UserDto>;
    public class GetUserByIdHandler : IRequestHandler<GetUserById, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("GetUserByIdHandler instance created.");
        }

        public async Task<UserDto> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetUserById command for User ID {UserId}...", request.UserId);
            try
            {
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                {
                    Log.Warning("User not found: ID={UserId}", request.UserId);
                    throw new NotFoundException($"User with ID {request.UserId} not found");
                }
                return _mapper.Map<UserDto>(user);
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "User not found");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve user");
                throw new Exception("Failed to retrieve user", ex);
            }
        }
    }

}
