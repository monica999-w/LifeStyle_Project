using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.IntegrationTests.Helpers
{
    public class DataContextBuilder : IDisposable
    {
        private readonly LifeStyleContext _dataContext;

        public DataContextBuilder(string dbName = "TestDatabase")
        {
            var options = new DbContextOptionsBuilder<LifeStyleContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                  .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)) 
                  .Options;

            var context = new LifeStyleContext(options);

            _dataContext = context;
        }

        public LifeStyleContext GetContext()
        {
            _dataContext.Database.EnsureCreated();
            return _dataContext;
        }

        public void SeedExercises(int number = 1)
        {
            var exercises = new List<Exercise>();

            for (int i = 0; i < number; i++)
            {
                var id = i + 1;

                exercises.Add(new Exercise
                {
                    ExerciseId = id,
                    Name = $"exercise-{id}",
                    DurationInMinutes = id * 10,
                    Type = Domain.Enums.ExerciseType.Pilates
                });
            }

            _dataContext.AddRange(exercises);
            _dataContext.SaveChanges();
        }

        public void SeedUsers(int number = 1)
        {
            var users = new List<UserProfile>();

            for (int i = 0; i < number; i++)
            {
                var id = i + 1;

                users.Add(new UserProfile
                {
                    ProfileId = id,
                    Email = $"email-{id}",
                    PhoneNumber = $"phone - {id}",
                    Height = id * 10,
                    Weight= id * 10,
                    UserId= $"id {id}",
                });
            }

            _dataContext.AddRange(users);
            _dataContext.SaveChanges();
        }
        public void SeedMeals(int number = 1)
        {
            var meals = new List<Meal>();

            for (int i = 0; i < number; i++)
            {
                var id = i + 1;

                meals.Add(new Meal
                {
                    MealId = id,
                    MealName = $"name-{id}",
                    MealType = Domain.Enums.MealType.Breakfast,
                    Nutrients = new Nutrients
                    {
                        Calories = 200 + i * 10,
                        Protein = 10 + i * 2,
                        Carbohydrates = 30 + i * 5,
                        Fat = 5 + i * 1
                    },
                    Image = $"/images/testmeal{i}.jpg",
                    PreparationInstructions = "Mix ingredients and cook for 15 minutes.",
                    EstimatedPreparationTimeInMinutes = 15 + i * 2,
                    Ingredients = new List<string> { "Ingredient1", "Ingredient2" },
                    Allergies = new List<AllergyType> { AllergyType.Gluten },
                    Diets = new List<DietType> { DietType.Vegan }
                });
            }

            _dataContext.AddRange(meals);
            _dataContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataContext.Dispose();
            }
        }
    }
}
