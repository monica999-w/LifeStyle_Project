using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Models.Exercises;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record DeleteExercise(int ExerciseId) : IRequest<Unit>;

    public class DeleteExerciseHandler : IRequestHandler<DeleteExercise, Unit>
    {
      
        private readonly IRepository<Exercise> _exerciseRepository;


        public DeleteExerciseHandler(IRepository<Exercise> exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<Unit> Handle(DeleteExercise request, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetById(request.ExerciseId);
            if (exercise == null)
            {
                throw new KeyNotFoundException("Exercise not found");
            }

            await _exerciseRepository.Remove(exercise);
            return Unit.Value;
        }
    }
}
