using LifeStyle.Aplication.Interfaces;
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
    public class DeleteMealHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IRequestHandler<DeleteNutrient, Nutrients> _deleteNutrientHandlerMock;

        public DeleteMealHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _deleteNutrientHandlerMock = Substitute.For<IRequestHandler<DeleteNutrient, Nutrients>>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsUnit()
        {
            // Arrange
            var repository = Substitute.For<IRepository<Meal>>();

            var handler = new DeleteMealHandler(_unitOfWorkMock, _deleteNutrientHandlerMock);

            var mealId = 1;
            var request = new DeleteMeal(mealId);

            var meal = new Meal
            {
                MealId = mealId,
                MealName = "Meal 1",
                MealType = MealType.Breakfast,
                Nutrients = new Nutrients(1, 200, 20, 30, 10)
            };

            _unitOfWorkMock.MealRepository.GetById(mealId).Returns(meal);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Unit>(result);
            await _unitOfWorkMock.MealRepository.Received(1).Remove(meal);
            await _unitOfWorkMock.Received(1).SaveAsync();
        }
    }
}
