using LifeStyle.Models.Planner;
using LifeStyle.Models.User;

namespace LifeStyle.Interfaces
{
    public interface IPlannerRepository
    {
        Task AddPlanner(Planner planner);
        Task RemovePlanner(Planner planner);
        Task<Planner> GetPlannerByUser(UserProfile profile);
        Task UpdatePlannerAsync(Planner planner);
    }
}
