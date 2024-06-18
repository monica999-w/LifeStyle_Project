using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Exercises.Responses;
using LifeStyle.Domain.Models.Exercises;
using MediatR;

namespace LifeStyle.Application.Exercises.Query
{
    public record FilterExercisesQuery(ExerciseFilterDto Filter) : IRequest<IEnumerable<Exercise>>;

    public class FilterExercisesQueryHandler : IRequestHandler<FilterExercisesQuery, IEnumerable<Exercise>>
    {
        private readonly IRepository<Exercise> _exerciseRepository;

        public FilterExercisesQueryHandler(IRepository<Exercise> exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<IEnumerable<Exercise>> Handle(FilterExercisesQuery request, CancellationToken cancellationToken)
        {
            var exercises = await _exerciseRepository.GetAll();

            if (request.Filter.Type.HasValue)
            {
                exercises = exercises.Where(e => e.Type == request.Filter.Type.Value).ToList();
            }

            if (request.Filter.Equipment.HasValue)
            {
                exercises = exercises.Where(e => e.Equipment == request.Filter.Equipment.Value).ToList();
            }

            if (request.Filter.MajorMuscle.HasValue)
            {
                exercises = exercises.Where(e => e.MajorMuscle == request.Filter.MajorMuscle.Value).ToList();
            }

            return exercises;
        }
    }
}
