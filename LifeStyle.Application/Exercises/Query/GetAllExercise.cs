using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Paged;
using MediatR;


namespace LifeStyle.Application.Exercises.Query
{
    public record GetAllExercise(int PageNumber, int PageSize) : IRequest<PagedResult<ExerciseDto>>;

    public class GetAllExercisesHandler : IRequestHandler<GetAllExercise, PagedResult<ExerciseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllExercisesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<ExerciseDto>> Handle(GetAllExercise request, CancellationToken cancellationToken)
        {
            var exercises = await _unitOfWork.ExerciseRepository.GetAll();
            var totalCount = exercises.Count();
            var items = exercises.Skip((request.PageNumber - 1) * request.PageSize)
                                 .Take(request.PageSize)
                                 .ToList();

            var exerciseDtos = _mapper.Map<List<ExerciseDto>>(items);
            return new PagedResult<ExerciseDto>(exerciseDtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
