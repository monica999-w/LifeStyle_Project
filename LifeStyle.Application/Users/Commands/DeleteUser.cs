using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;



namespace LifeStyle.Application.Users.Commands
{
    public record DeleteUser(int UserId) : IRequest<Unit>;

    public class DeleteUserHandler : IRequestHandler<DeleteUser, Unit>
    {

        private readonly IUnitOfWork _unitOfWork;


        public DeleteUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteUser request, CancellationToken cancellationToken)
        {

            try
            {
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);

                if (user == null)
                {
                    throw new NotFoundException("User not found");
                }

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.UserProfileRepository.Remove(user);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                return Unit.Value;
            }
            catch(NotFoundException ex)
            {
                throw ex;
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Failed to delete user", ex);
            }
        }
    }
}

