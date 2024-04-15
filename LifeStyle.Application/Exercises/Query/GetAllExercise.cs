using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Exercises.Query
{
    public record GetAllExercise : IRequest<List<ExerciseDto>>;
    public class GetAllExercisesHandler : IRequestHandler<GetAllExercise, List<ExerciseDto>>
    {
        private readonly IRepository<Exercise> _exerciseRepository;

        public GetAllExercisesHandler(IRepository<Exercise> exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<List<ExerciseDto>> Handle(GetAllExercise request, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetAll(); 
            return exercise.Select(ExerciseDto.FromExercise).ToList();
        }
    }


}
