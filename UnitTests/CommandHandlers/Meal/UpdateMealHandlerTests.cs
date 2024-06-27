using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Services;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using NSubstitute;


namespace LifeStyle.UnitTests.CommandHandlers
{
    public class UpdateMealHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IRequestHandler<UpdateNutrient, Nutrients> _updateNutrientHandlerMock;
        private readonly IFileService _fileServiceMock;


        public UpdateMealHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _updateNutrientHandlerMock = Substitute.For<IRequestHandler<UpdateNutrient, Nutrients>>();
            _fileServiceMock = Substitute.For<IFileService>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsUpdatedMeal()
        {
            // Arrange
            var handler = new UpdateMealHandler(_unitOfWorkMock, _updateNutrientHandlerMock,_fileServiceMock);

            var mealId = 1;
            var request = new UpdateMeal(mealId,
                "Meal 1",
                MealType.Breakfast,
                new Nutrients(1, 200, 20, 30, 10),
                new List<string> { "Ingredient 1", "Ingredient 2" },
                "Preparation instructions...",
                30,
                null,
                new List<AllergyType> { AllergyType.Gluten },
                new List<DietType> { DietType.Vegetarian }
                );

            var existingMeal = new Meal
            {
                MealId = mealId,
                MealName = "Meal",
                MealType = MealType.Breakfast,
                Nutrients = new Nutrients(1, 200, 20, 30, 10)
            };

            _unitOfWorkMock.MealRepository.GetById(mealId).Returns(existingMeal);

            _updateNutrientHandlerMock.Handle(Arg.Any<UpdateNutrient>(), Arg.Any<CancellationToken>())
                .Returns(request.Nutrients);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.MealId, result.MealId);
            Assert.Equal(request.Name, result.MealName);
            Assert.Equal(request.MealType, result.MealType);
            Assert.Equal(request.Nutrients, result.Nutrients);

            await _unitOfWorkMock.MealRepository.Received(1).Update(Arg.Is<Meal>(m =>
                m.MealId == request.MealId &&
                m.MealName == request.Name &&
                m.MealType == request.MealType &&
                m.Nutrients == request.Nutrients
            ));
            await _unitOfWorkMock.Received(1).SaveAsync();
        }
    }
}
