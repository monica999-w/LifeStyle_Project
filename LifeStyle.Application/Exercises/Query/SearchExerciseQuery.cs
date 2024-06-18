using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Exercises.Query
{
    public record SearchExerciseQuery(string SearchTerm) : IRequest<IEnumerable<Exercise>>;

    public class SearchExerciseQueryHandler : IRequestHandler<SearchExerciseQuery, IEnumerable<Exercise>>
    {
        private readonly IRepository<Exercise> _exerciseRepository;

        public SearchExerciseQueryHandler(IRepository<Exercise> exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<IEnumerable<Exercise>> Handle(SearchExerciseQuery request, CancellationToken cancellationToken)
        {
            return await _exerciseRepository.SearchAsync(request.SearchTerm);
        }
    }
}
