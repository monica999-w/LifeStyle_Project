using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Serilog;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public GetAllExercisesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("GetAllExercisesHandler instance created.");
        }

        public async Task<List<ExerciseDto>> Handle(GetAllExercise request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAllExercise command...");

            try
            {
                var exercises = await _unitOfWork.ExerciseRepository.GetAll();
                return _mapper.Map<List<ExerciseDto>>(exercises);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve all exercises");
                throw new Exception("Failed to retrieve all exercises", ex);
            }
        }

    }


}
