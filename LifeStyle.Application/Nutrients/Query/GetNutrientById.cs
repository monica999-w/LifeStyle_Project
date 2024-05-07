using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using MediatR;


namespace LifeStyle.Application.Query
{
    public record GetNutrientById(int NutrientId) : IRequest<NutrientDto>;
    public class GetNutrientByIdHandler : IRequestHandler<GetNutrientById, NutrientDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNutrientByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NutrientDto> Handle(GetNutrientById request, CancellationToken cancellationToken)
        {
            try
            {
                var nutrient = await _unitOfWork.NutrientRepository.GetById(request.NutrientId);
                if (nutrient == null)
                    throw new NotFoundException($"Nutrient with ID {request.NutrientId} not found");

                return NutrientDto.FromNutrient(nutrient);
            }
            catch (NotFoundException ex)
            {
                throw;
            }
        }
    }
}

