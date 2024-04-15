using LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;

namespace LifeStyle.Aplication.Interfaces
{
    public interface IPlannerRepository
    {
        Task AddPlanner(Planner planner);
        Task RemovePlanner(Planner planner);
        Task<Planner> GetPlannerByUser(UserProfile profile);
        Task UpdatePlannerAsync(Planner planner);
        Task<IEnumerable<Planner>> GetAll();

    }
}
