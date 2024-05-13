using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Query
{
    public record GetExerciseById(int ExerciseId) : IRequest<ExerciseDto>;
    public class GetExerciseByIdHandler : IRequestHandler<GetExerciseById, ExerciseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetExerciseByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("GetExerciseByIdHandler instance created.");
        }

        public async Task<ExerciseDto> Handle(GetExerciseById request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetExerciseById command for Exercise ID {ExerciseId}...", request.ExerciseId);

            try
            {
                var exercise = await _unitOfWork.ExerciseRepository.GetById(request.ExerciseId);
                if (exercise == null)
                {
                    Log.Warning("Exercise not found: ID={ExerciseId}", request.ExerciseId);
                    throw new NotFoundException($"Exercise with ID {request.ExerciseId} not found");
                }

                return _mapper.Map<ExerciseDto>(exercise);
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Exercise not found");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve exercise");
                throw new Exception("Failed to retrieve exercise", ex);
            }
        }

    }
}
