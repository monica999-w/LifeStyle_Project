using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Exercises.Responses;
using LifeStyle.Domain.Models.Exercises;
using MediatR;

namespace LifeStyle.Application.Exercises.Query
{
    public record FilterExercisesQuery(Exercise Filter) : IRequest<IEnumerable<Exercise>>;

    public class FilterExercisesQueryHandler : IRequestHandler<FilterExercisesQuery, IEnumerable<Exercise>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterExercisesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Exercise>> Handle(FilterExercisesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ExerciseRepository.Filter(request.Filter);
        }
    }


}
