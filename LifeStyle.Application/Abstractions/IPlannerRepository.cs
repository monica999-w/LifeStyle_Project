using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;

namespace LifeStyle.Aplication.Interfaces
{
    public interface IPlannerRepository
    {
        Task<Planner> AddPlanner(Planner planner);
        Task<Planner> RemovePlanner(Planner planner);
        Task<Planner> GetPlannerByUser(UserProfile profile);
        Task<Planner> GetPlannerById(int id);
        Task<Planner> UpdatePlannerAsync(Planner planner);
        Task<IEnumerable<Planner>> GetAll();

        Task RemoveMealAsync(Meal meal);
        Task RemoveExerciseAsync(Exercise exercise);
        Task<UserProfile?> GetByEmail(string email);
        Task<Planner?> GetByUser(int userId);
        Task<Planner?> GetPlannerByDate(int userId, DateTime date);
        Task<IEnumerable<DateTime>> GetAvailablePlannerDates(int userId);
    }
}
