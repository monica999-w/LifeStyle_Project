using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Models.Exercises;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Exercises.Query
{
    public record GetAllExercise : IRequest<List<Exercise>>;
    public class GetAllExercisesHandler : IRequestHandler<GetAllExercise, List<Exercise>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetAllExercisesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("GetAllExercisesHandler instance created.");
        }

        public async Task<List<Exercise>> Handle(GetAllExercise request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAllExercise command...");

            try
            {
                var exercises = await _unitOfWork.ExerciseRepository.GetAll();
                return exercises;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve all exercises");
                throw new Exception("Failed to retrieve all exercises", ex);
            }
        }
    }
}
