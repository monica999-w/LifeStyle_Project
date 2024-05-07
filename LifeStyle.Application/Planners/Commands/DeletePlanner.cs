using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;


namespace LifeStyle.Application.Planners.Commands
{
    public record DeletePlanner(int UserId, int? MealId = null, int? ExerciseId = null) : IRequest<bool>;

    public class DeletePlannerHandler : IRequestHandler<DeletePlanner, bool>
    {
        private readonly IUnitOfWork _unitOfWork;


        public DeletePlannerHandler(IUnitOfWork unitOfWork)
        {
          _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePlanner request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {request.UserId} not found");
                }

                Meal mealToRemove = null;
                Exercise exerciseToRemove = null;

                if (request.MealId.HasValue)
                {
                    mealToRemove = await _unitOfWork.MealRepository.GetById(request.MealId.Value);
                    if (mealToRemove == null)
                    {
                        throw new NotFoundException($"Meal with ID {request.MealId} not found");
                    }
                }

                if (request.ExerciseId.HasValue)
                {
                    exerciseToRemove = await _unitOfWork.ExerciseRepository.GetById(request.ExerciseId.Value);
                    if (exerciseToRemove == null)
                    {
                        throw new NotFoundException($"Exercise with ID {request.ExerciseId} not found");
                    }
                }

                var planner = await _unitOfWork.PlannerRepository.GetPlannerByUser(user);
                if (planner == null)
                {
                    throw new NotFoundException($"Planner not found for user with ID {user.ProfileId}");
                }

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
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Failed to delete planner", ex);
            }
        }

    }

}