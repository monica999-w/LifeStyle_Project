﻿using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Meals.Query
{
    public record GetMealById(int MealId) : IRequest<MealDto>;

    public class GetMealByIdHandler : IRequestHandler<GetMealById, MealDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMealByIdHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("GetMealByIdHandler instance created.");
        }

        public async Task<MealDto> Handle(GetMealById request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetMealByIdHandler command for Meal ID {MealId}...", request.MealId);
            try
            {
                var meal = await _unitOfWork.MealRepository.GetById(request.MealId);
                if (meal == null)
                {
                    Log.Warning("Meal not found: ID={MealId}", request.MealId);
                    throw new NotFoundException($"Meal with ID {request.MealId} not found");
                }

                return _mapper.Map<MealDto>(meal);

            }
            catch(NotFoundException ex) 
            {
                Log.Error(ex, "Meal not found");
                throw;
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Failed to retrieve meal");
                throw new Exception("Failed to retrieve meal", ex);
            }
        }
    }

}
