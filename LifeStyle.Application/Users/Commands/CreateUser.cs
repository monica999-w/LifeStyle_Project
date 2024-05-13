using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record CreateUser( string Email ,string PhoneNumber, double Height, double Weight): IRequest<UserDto>;
    public  class CreateUserHander : IRequestHandler<CreateUser,UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserHander(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("CreateUserHander instance created.");
          
        }

        public async Task<UserDto> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            Log.Information("Handling CreateExercise command...");
            try
            {
                Log.Information("Creating exercise: Email={Email}, PhoneNumber={PhoneNumber}, Height={Height}, Weight={Weight}", request.Email, request.PhoneNumber, request.Height,request.Weight);
                var existingUser = await _unitOfWork.UserProfileRepository.GetByName(request.Email);
                if (existingUser != null)
                {
                    Log.Warning("Exercise with the same name already exists: Email={Email}", request.Email);
                    throw new AlreadyExistsException("User already exists");
                }
                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                var newUser = new UserProfile
                {
                    Email=request.Email,
                    PhoneNumber=request.PhoneNumber,
                    Height=request.Height,
                    Weight=request.Weight

                };
                Log.Information("Adding user to repository...");
                await _unitOfWork.UserProfileRepository.Add(newUser);
                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("User created successfully: ID={ExerciseId}", newUser.ProfileId);

                return _mapper.Map<UserDto>(newUser);
            }
            catch(AlreadyExistsException ex)
            {
                Log.Error(ex, "Failed to create user: User already exists");
                throw ;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create user");
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Failed to create user", ex);
            }
        }
    }
}
