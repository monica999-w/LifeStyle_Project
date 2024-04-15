using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using MediatR;


namespace LifeStyle.Application.Query
{
    public record GetExerciseById(int ExerciseId) : IRequest<ExerciseDto>;
    public class GetExerciseByIdHandler : IRequestHandler<GetExerciseById, ExerciseDto>
    {
        private readonly IRepository<Exercise> _exerciseRepository;

        public GetExerciseByIdHandler(IRepository<Exercise> exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<ExerciseDto> Handle(GetExerciseById request, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetById(request.ExerciseId);
            if (exercise == null)
                throw new Exception($"Exercise with ID {request.ExerciseId} not found");

            return ExerciseDto.FromExercise(exercise);
        }
    }
}
