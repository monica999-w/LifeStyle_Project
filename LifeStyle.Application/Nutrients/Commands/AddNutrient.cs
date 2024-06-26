﻿using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record CreateNutrient(double Calories,  double Protein, double Carbohydrates , double Fat ) : IRequest<Nutrients>;

    public class CreateNutrientHandler : IRequestHandler<CreateNutrient, Nutrients>
    {

        private readonly IUnitOfWork _unitOfWork;
        

        public CreateNutrientHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
         
            Log.Information("CreateNutrientHandler instance created.");
        }

        public async Task<Nutrients> Handle(CreateNutrient request, CancellationToken cancellationToken)
        {
            Log.Information("Handling CreateNutrient command...");
            try
            {
                var newNutrient = new Nutrients
                {
                        Calories = request.Calories,
                        Protein = request.Protein,
                        Carbohydrates = request.Carbohydrates,
                        Fat = request.Fat
                        
                };

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                Log.Information("Adding newNutrient to repository...");
                await _unitOfWork.NutrientRepository.Add(newNutrient);
           

                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
               

                return newNutrient;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create nutrient");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to create nutrient", ex);
            }

        }
    }
}