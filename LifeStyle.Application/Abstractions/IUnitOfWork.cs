using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;


namespace LifeStyle.Application.Abstractions
{
    public interface IUnitOfWork: IDisposable
    {
        public IRepository<Exercise> ExerciseRepository { get; }
        public IRepository<Nutrients> NutrientRepository { get; }
        public IRepository<Meal> MealRepository { get; }
        public IRepository<UserProfile> UserProfileRepository { get; }
        public IPlannerRepository PlannerRepository { get; }
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task BeginTransactionAsync();
        Task SaveAsync();
    }
}
