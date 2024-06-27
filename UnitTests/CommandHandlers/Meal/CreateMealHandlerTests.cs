using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Services;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;


namespace LifeStyle.UnitTests.CommandHandlers
{
    public class CreateMealHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IRequestHandler<CreateNutrient, Nutrients> _createNutrientHandlerMock;
        private readonly IFileService _fileServiceMock;

        public CreateMealHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _createNutrientHandlerMock = Substitute.For<IRequestHandler<CreateNutrient, Nutrients>>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsMeal()
        {
            // Arrange
            var repository = Substitute.For<IRepository<Meal>>();

            var handler = new CreateMealHandler(_unitOfWorkMock, _createNutrientHandlerMock,_fileServiceMock);

            var createdAt = DateTime.UtcNow; 
            var request = new CreateMeal(
                "Meal 1",
                MealType.Breakfast,
                new Nutrients(1, 200, 20, 30, 10),
                new List<string> { "Ingredient 1", "Ingredient 2" },
                "Preparation instructions...",
                30,
                new List<AllergyType> { AllergyType.Gluten },
                new List<DietType> { DietType.Vegetarian },
                null
            );


            repository?.GetByName(Arg.Any<string>()).Returns((Meal)null);
            _unitOfWorkMock.MealRepository.Returns(repository);

            _createNutrientHandlerMock.Handle(Arg.Any<CreateNutrient>(), Arg.Any<CancellationToken>())
                .Returns(new Nutrients(1, 200, 20, 30, 10));

            repository.When(repo => repo.Add(Arg.Any<Meal>()))
                .Do(info =>
                {
                    var mealArg = info.Arg<Meal>();
                    Assert.Equal(request.MealName, mealArg.MealName);
                    Assert.Equal(request.MealType, mealArg.MealType);
                    Assert.Equal(200, mealArg.Nutrients.Calories);
                    Assert.Equal(20, mealArg.Nutrients.Protein);
                    Assert.Equal(30, mealArg.Nutrients.Carbohydrates);
                    Assert.Equal(10, mealArg.Nutrients.Fat);
                });

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Meal>(result);
            await repository.Received(1).Add(Arg.Any<Meal>());
            await _unitOfWorkMock.Received(1).SaveAsync();
        }
    }
}

