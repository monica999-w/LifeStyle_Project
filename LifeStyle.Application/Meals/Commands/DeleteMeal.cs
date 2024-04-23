﻿using LifeStyle.Application.Abstractions;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record DeleteMeal(int MealId) : IRequest<Unit>;

    public class DeleteMealHandler : IRequestHandler<DeleteMeal, Unit>
    {

        private readonly IUnitOfWork _unitOfWork;


        public DeleteMealHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteMeal request, CancellationToken cancellationToken)
        {

            try
            {
                var meal = await _unitOfWork.MealRepository.GetById(request.MealId);

                if (meal == null)
                {
                    throw new Exception("Meal not found");
                }

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.MealRepository.Remove(meal);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                return Unit.Value;
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to delete meal", ex);
            }
        }
    }
}
