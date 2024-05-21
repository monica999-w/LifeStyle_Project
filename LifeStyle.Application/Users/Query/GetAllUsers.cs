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
    public record GetAllUsers : IRequest<List<UserProfile>>;
    public class GetAllUsersHandler : IRequestHandler<GetAllUsers, List<UserProfile>>
    {
        private readonly IUnitOfWork _unitOfWork;
       

        public GetAllUsersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("GetAllUsersHandler instance created.");
        }

        public async Task<List<UserProfile>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAllUsers command...");
            try
            {
                var users = await _unitOfWork.UserProfileRepository.GetAll();
                return users;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve all users");
                throw new Exception("Failed to retrieve all users", ex);
            }
        }
    }


}
