using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record DeleteNutrient(int NutrientId) : IRequest<Unit>;

    public class DeleteNutrientHandler : IRequestHandler<DeleteNutrient, Unit>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;   



        public DeleteNutrientHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("DeleteNutrientHandler instance created.");
        }

        public async Task<Unit> Handle(DeleteNutrient request, CancellationToken cancellationToken)
        {

            Log.Information("Handling DeleteNutrient command...");
            try
            {
                Log.Information("Deleting nutrient with Id {NutrientId}", request.NutrientId);
                var nutrient = await _unitOfWork.NutrientRepository.GetById(request.NutrientId);

                if (nutrient == null)
                {
                    Log.Warning("Nutrient not found: ID={NutrientId}", request.NutrientId);
                    throw new NotFoundException("Nutrient not found");
                }
                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.NutrientRepository.Remove(nutrient);

                await _unitOfWork.SaveAsync();

                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();

                Log.Information("Nutrient deleted successfully: ID={NutrientId}", request.NutrientId);

                return Unit.Value;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Nutrient not found");
                throw;
            }

            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete nutrirnt");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to delete nutrient", ex);
            }
        }
    }

}
