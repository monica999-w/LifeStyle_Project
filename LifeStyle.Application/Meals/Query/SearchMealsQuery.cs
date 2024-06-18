using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Meals.Query
{
    public record SearchMealsQuery(string SearchTerm) : IRequest<IEnumerable<Meal>>;

    public class SearchMealsQueryHandler : IRequestHandler<SearchMealsQuery, IEnumerable<Meal>>
    {
        private readonly IRepository<Meal> _mealRepository;

        public SearchMealsQueryHandler(IRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<IEnumerable<Meal>> Handle(SearchMealsQuery request, CancellationToken cancellationToken)
        {
            return await _mealRepository.SearchAsync(request.SearchTerm);
        }
    }

}
