using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record CreateExercise(string Name, int DurationInMinutes, ExerciseType Type) : IRequest<ExerciseDto>;

    public class CreateExerciseHandler : IRequestHandler<CreateExercise, ExerciseDto>
    {
        private readonly IRepository<Exercise> _exerciseRepository;


        public CreateExerciseHandler(IRepository<Exercise> exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<ExerciseDto> Handle(CreateExercise request, CancellationToken cancellationToken)
        {

            var exercise = new Exercise
            (
               id: GetNextId(),
               name: request.Name,
               durationInMinutes: request.DurationInMinutes,
               type: request.Type
            );

            await _exerciseRepository.Add(exercise);
            return ExerciseDto.FromExercise(exercise);
        }
        private int GetNextId()
        {
            var lastId = _exerciseRepository.GetLastId();
            return lastId + 1; 
        }
    }
}
