using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.UnitTests.CommandHandlers
{
    public class UpdateMealHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IRequestHandler<UpdateNutrient, Nutrients> _updateNutrientHandlerMock;

        public UpdateMealHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _updateNutrientHandlerMock = Substitute.For<IRequestHandler<UpdateNutrient, Nutrients>>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsUpdatedMeal()
        {
            // Arrange
            var handler = new UpdateMealHandler(_unitOfWorkMock, _updateNutrientHandlerMock);

            var mealId = 1;
            var request = new UpdateMeal(mealId, "Updated Meal", MealType.Lunch, new Nutrients(1,300, 25, 40, 15));

            var existingMeal = new Meal
            {
                MealId = mealId,
                Name = "Meal",
                MealType = MealType.Breakfast,
                Nutrients = new Nutrients(1,200, 20, 30, 10)
            };

            _unitOfWorkMock.MealRepository.GetById(mealId).Returns(existingMeal);

            _updateNutrientHandlerMock.Handle(Arg.Any<UpdateNutrient>(), Arg.Any<CancellationToken>())
                .Returns(request.Nutrients);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.MealId, result.MealId);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.MealType, result.MealType);
            Assert.Equal(request.Nutrients, result.Nutrients);

            await _unitOfWorkMock.MealRepository.Received(1).Update(Arg.Is<Meal>(m =>
                m.MealId == request.MealId &&
                m.Name == request.Name &&
                m.MealType == request.MealType &&
                m.Nutrients == request.Nutrients
            ));
            await _unitOfWorkMock.Received(1).SaveAsync();
        }
    }
}
