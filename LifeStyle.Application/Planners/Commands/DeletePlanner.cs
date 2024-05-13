using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Planners.Commands
{
    public record DeletePlanner(int UserId, int? MealId = null, int? ExerciseId = null) : IRequest<bool>;

    public class DeletePlannerHandler : IRequestHandler<DeletePlanner, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;



        public DeletePlannerHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("DeletePlannerHandler instance created.");
        }

        public async Task<bool> Handle(DeletePlanner request, CancellationToken cancellationToken)
        {
            Log.Information("Handling DeletePlanner command...");

            try
            {
                Log.Information("Deleting planner...");
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                {
                    Log.Warning("Planner with User Id not found: ID={UserId}", request.UserId);
                    throw new NotFoundException($"User with ID {request.UserId} not found");
                }

                Meal mealToRemove = null;
                Exercise exerciseToRemove = null;

                if (request.MealId.HasValue)
                {
                    mealToRemove = await _unitOfWork.MealRepository.GetById(request.MealId.Value);
                    if (mealToRemove == null)
                    {
                        Log.Warning("Planner with Meal Id not found: ID={MealId}", request.MealId);
                        throw new NotFoundException($"Meal with ID {request.MealId} not found");
                    }
                }

                if (request.ExerciseId.HasValue)
                {
                    exerciseToRemove = await _unitOfWork.ExerciseRepository.GetById(request.ExerciseId.Value);
                    if (exerciseToRemove == null)
                    {
                        Log.Warning("Planner with Exercise Id not found: ID={ExerciseId}", request.ExerciseId);
                        throw new NotFoundException($"Exercise with ID {request.ExerciseId} not found");
                    }
                }

                var planner = await _unitOfWork.PlannerRepository.GetPlannerByUser(user);
                if (planner == null)
                {
                    throw new NotFoundException($"Planner not found for user with ID {user.ProfileId}");
                }

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                if (mealToRemove != null)
                {
                    await _unitOfWork.PlannerRepository.RemoveMealAsync(mealToRemove);
                }

                if (exerciseToRemove != null)
                {
                    await _unitOfWork.PlannerRepository.RemoveExerciseAsync(exerciseToRemove);
                }

                if (mealToRemove == null && exerciseToRemove == null)
                {
                    await _unitOfWork.PlannerRepository.RemovePlanner(planner);
                }
                else
                {
                    await _unitOfWork.PlannerRepository.UpdatePlannerAsync(planner);
                }

                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("Planner deleted successfully");

                return true;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Not found exception");
                throw ;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete planner");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to delete planner", ex);
            }
        }

    }

}