using LifeStyle.Application.Abstractions;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record DeleteNutrient(int NutrientId) : IRequest<Unit>;

    public class DeleteNutrientHandler : IRequestHandler<DeleteNutrient, Unit>
    {

        private readonly IUnitOfWork _unitOfWork;


        public DeleteNutrientHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteNutrient request, CancellationToken cancellationToken)
        {

            try
            {
                var nutrient = await _unitOfWork.NutrientRepository.GetById(request.NutrientId);

                if (nutrient == null)
                {
                    throw new Exception("Nutrient not found");
                }

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.NutrientRepository.Remove(nutrient);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                return Unit.Value;
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to delete nutrient", ex);
            }
        }
    }

}
