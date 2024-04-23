using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using MediatR;


namespace LifeStyle.Application.Query
{
    public record GetExerciseById(int ExerciseId) : IRequest<ExerciseDto>;
    public class GetExerciseByIdHandler : IRequestHandler<GetExerciseById, ExerciseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetExerciseByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ExerciseDto> Handle(GetExerciseById request, CancellationToken cancellationToken)
        {
            var exercise = await _unitOfWork.ExerciseRepository.GetById(request.ExerciseId);
            if (exercise == null)
                throw new Exception($"Exercise with ID {request.ExerciseId} not found");

            return ExerciseDto.FromExercise(exercise);
        }
    }
}
