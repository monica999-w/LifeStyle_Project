using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.UnitTests.QueryHandlers
{
    public class GetExerciseByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingExerciseId_ReturnsExerciseDto()
        {
            // Arrange
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var handler = new GetExerciseByIdHandler(unitOfWork);
            var exerciseId = 1;

            var expectedExercise = new Exercise
            {
                ExerciseId = exerciseId,
                Name = "Exercise 1",
                DurationInMinutes = 30,
                Type = ExerciseType.Yoga
            };

            unitOfWork.ExerciseRepository.GetById(exerciseId).Returns(expectedExercise);

            // Act
            var result = await handler.Handle(new GetExerciseById(exerciseId), default);

            // Assert
            Assert.NotNull(result); 
            Assert.Equal(expectedExercise.ExerciseId, result.ExerciseId);
            Assert.Equal(expectedExercise.Name, result.Name); 
            Assert.Equal(expectedExercise.DurationInMinutes, result.DurationInMinutes);
            Assert.Equal(expectedExercise.Type, result.Type); 
        }
    

        [Fact]
        public async Task Handle_NonExistingExerciseId_ThrowsNotFoundException()
        {
            //Arrange
           var unitOfWork = Substitute.For<IUnitOfWork>();
            var mapper = Substitute.For<IMapper>();

            var handler = new GetExerciseByIdHandler(unitOfWork);
            var request = new GetExerciseById(4);

            unitOfWork.ExerciseRepository.GetById(request.ExerciseId).Returns((Exercise)null);

           // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, default));

        }

        [Fact]
        public async Task Handle_ExceptionThrown_LogsAndThrowsException()
        {
          //  Arrange
           var unitOfWork = Substitute.For<IUnitOfWork>();
            var mapper = Substitute.For<IMapper>();

            var handler = new GetExerciseByIdHandler(unitOfWork);
            var request = new GetExerciseById(4);
            var exception = new Exception("Test exception");
            unitOfWork.ExerciseRepository.GetById(request.ExerciseId).Throws(exception);

            //Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, default));

        }
    }
}
