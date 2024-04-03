using LifeStyle.LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;

namespace LifeStyle.LifeStyle.Aplication.Interfaces
{
    public interface IPlannerRepository
    {
        Task AddPlanner(Planner planner);
        Task RemovePlanner(Planner planner);
        Task<Planner> GetPlannerByUser(UserProfile profile);
        Task UpdatePlannerAsync(Planner planner);
    }
}
