using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Models.Planner;

namespace LifeStyle.Aplication.Logic
{
    public class PlannerRepository : IPlannerRepository
    {
        private readonly List<Planner>? _planners = new List<Planner>();

        
        public Task AddPlanner(Planner planner)
        {
            _planners?.Add(planner);
            return Task.CompletedTask;
        }

        public Task RemovePlanner(Planner planner)
        {
            _planners?.Remove(planner);
            return Task.CompletedTask;
        }

        public Task<Planner> GetPlannerByUser(UserProfile profile)
        {
            return Task.FromResult(_planners.FirstOrDefault(p => p.Profile == profile));
        }

        public async Task<IEnumerable<Planner>> GetAll()
        {
            await Task.Delay(0);
            return _planners;
        }


        public async Task UpdatePlannerAsync(Planner planner)
        {

            var existingPlanner = await GetPlannerByUser(planner.Profile);
            if (existingPlanner != null)
            {
                existingPlanner.Meals = planner.Meals;
                existingPlanner.Exercises = planner.Exercises;

            }

        }
    }
}

