using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Query
{
    public record GetNutrientById(int NutrientId) : IRequest<NutrientDto>;
    public class GetNutrientByIdHandler : IRequestHandler<GetNutrientById, NutrientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetNutrientByIdHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("GetNutrientByIdHandler instance created.");
        }
    

        public async Task<NutrientDto> Handle(GetNutrientById request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetNutrientByIdHandler command for Nutrient ID {Nutrient}...", request.NutrientId);

            try
            {
                var nutrient = await _unitOfWork.NutrientRepository.GetById(request.NutrientId);
                if (nutrient == null)
                {
                    Log.Warning("Meal not found: ID={NutrientId}", request.NutrientId);
                    throw new NotFoundException($"Nutrient with ID {request.NutrientId} not found");
                }

                return _mapper.Map<NutrientDto>(nutrient);
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Nutrient not found");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve nutrient");
                throw new Exception("Failed to retrieve nutrient", ex);
            }
        }
    }
}

