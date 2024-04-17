using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;


namespace LifeStyle.Application.Planners.Commands
{
    public record DeletePlanner(int UserId, int? MealId = null, int? ExerciseId = null) : IRequest<bool>;

    public class DeletePlannerHandler : IRequestHandler<DeletePlanner, bool>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IRepository<UserProfile> _userRepository;
        private readonly IRepository<Meal> _mealRepository;
        private readonly IRepository<Exercise> _exerciseRepository;

        public DeletePlannerHandler(IPlannerRepository plannerRepository, IRepository<UserProfile> userRepository, IRepository<Meal> mealRepository, IRepository<Exercise> exerciseRepository)
        {
            _plannerRepository = plannerRepository;
            _userRepository = userRepository;
            _mealRepository = mealRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<bool> Handle(DeletePlanner request, CancellationToken cancellationToken)
        {
            
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found");

            Meal? mealToRemove = null;
            Exercise? exerciseToRemove = null;

            if (request.MealId.HasValue)
            {
                mealToRemove = await _mealRepository.GetById(request.MealId.Value);
                if (mealToRemove == null)
                    throw new Exception($"Meal with ID {request.MealId} not found");
            }

            if (request.ExerciseId.HasValue)
            {
                exerciseToRemove = await _exerciseRepository.GetById(request.ExerciseId.Value);
                if (exerciseToRemove == null)
                    throw new Exception($"Exercise with ID {request.ExerciseId} not found");
            }

          
            var planner = await _plannerRepository.GetPlannerByUser(user);
            if (planner == null)
                throw new Exception($"Planner not found for user with ID {user.ProfileId}");

            
            if (mealToRemove != null)
            {
                planner.RemoveMeal(mealToRemove);
            }

            if (exerciseToRemove != null)
            {
                planner.RemoveExercise(exerciseToRemove);
            }

            if (mealToRemove == null && exerciseToRemove == null)
            {
                
                await _plannerRepository.RemovePlanner(planner);
            }
            else
            {
                
                await _plannerRepository.UpdatePlannerAsync(planner);
            }

            return true;
        }
    }

}