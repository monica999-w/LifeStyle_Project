using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.Context;


namespace LifeStyle.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LifeStyleContext _lifeStyleContext;
        public UnitOfWork(LifeStyleContext lifeStyleContext, IRepository<Exercise> exerciseRepository, IRepository<Nutrients> nutrientRepository, IRepository<Meal> mealRepository,IRepository<UserProfile> userProfileRepository,IPlannerRepository plannerRepository)
        {
            _lifeStyleContext = lifeStyleContext;
            ExerciseRepository = exerciseRepository;
            NutrientRepository = nutrientRepository;
            MealRepository = mealRepository;
            UserProfileRepository = userProfileRepository;
            PlannerRepository = plannerRepository;

        }

        public IRepository<Exercise> ExerciseRepository {  get; private set; }
        public IRepository<Nutrients> NutrientRepository { get; private set; }

        public IRepository<Meal> MealRepository { get; private set; }

        public IRepository<UserProfile> UserProfileRepository { get; private set; }

        public IPlannerRepository PlannerRepository { get; private set; }

        public async Task BeginTransactionAsync()
        {
            await _lifeStyleContext.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            await _lifeStyleContext.Database.CommitTransactionAsync();
        }

        public async void Dispose()
        {
            _lifeStyleContext.Dispose();
        }


        public async Task RollbackTransactionAsync()
        {
            await _lifeStyleContext.Database.RollbackTransactionAsync();
        }

        public async Task SaveAsync()
        {
           await _lifeStyleContext.SaveChangesAsync();
        }
    }
}
