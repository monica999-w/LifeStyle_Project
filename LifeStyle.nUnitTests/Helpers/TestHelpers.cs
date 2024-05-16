using AutoMapper;
using LifeStyle.Aplication.Logic;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Infrastructure.Context;
using LifeStyle.Infrastructure.Repository;
using LifeStyle.Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.Extensions.DependencyInjection;



namespace LifeStyle.IntegrationTests.Helpers
{
    public static class TestHelpers
    {
        public static IMapper CreateMapper()
        {
            var services = new ServiceCollection();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMapper>();
        }

        public static IMediator CreateMediator(LifeStyleContext dbContext)
        {
            var exerciseRepository = new ExerciseRepository(dbContext);
            var mealRepository = new MealRepository(dbContext); 
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);

            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var services = new ServiceCollection();
            services.AddMediatR(typeof(CreateExercise));
            services.AddSingleton(unitOfWork);
            services.AddScoped<IRequestHandler<CreateExercise, ExerciseDto>, CreateExerciseHandler>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMediator>();
        }

    }
}
