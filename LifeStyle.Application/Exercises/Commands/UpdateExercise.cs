using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record UpdateExercise(int ExerciseId, string Name, int DurationInMinutes, ExerciseType Type) : IRequest<ExerciseDto>;

    public class UpdateExerciseHandler : IRequestHandler<UpdateExercise, ExerciseDto>
    {
      
        private readonly IRepository<Exercise> _exerciseRepository;

        public UpdateExerciseHandler(IRepository<Exercise> exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<ExerciseDto> Handle(UpdateExercise request, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetById(request.ExerciseId);
            if (exercise == null)
            {
                throw new KeyNotFoundException($"Exercise with ID {request.ExerciseId} not found");
            }

            exercise.Name = request.Name;
            exercise.DurationInMinutes = request.DurationInMinutes;
            exercise.Type = request.Type;

            await _exerciseRepository.Update(exercise);
            return ExerciseDto.FromExercise(exercise);
        }
    }
}
