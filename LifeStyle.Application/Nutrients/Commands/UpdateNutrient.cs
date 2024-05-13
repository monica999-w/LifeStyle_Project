﻿using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Commands
{
    public record UpdateNutrient(int NutrientId,double Calories, double Protein, double Carbohydrates, double Fat) : IRequest<NutrientDto>;

    public class UpdateNutrientHandler : IRequestHandler<UpdateNutrient, NutrientDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateNutrientHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            Log.Information("UpdateNutrientHandler instance created.");
        }

        public async Task<NutrientDto> Handle(UpdateNutrient request, CancellationToken cancellationToken)
        {
            Log.Information("Handling UpdateNutrient command...");

            try
            {
                Log.Information("Updating nutrient with Id {NutrientId}", request.NutrientId);
                var nutrient = await _unitOfWork.NutrientRepository.GetById(request.NutrientId);
                if (nutrient == null)
                {
                    Log.Warning("Nutrient not found: ID={NutrientId}", request.NutrientId);
                    throw new NotFoundException($"Nutrient with ID {request.NutrientId} not found");
                }

                nutrient.Protein=request.Protein;
                nutrient.Calories=request.Calories;
                nutrient.Carbohydrates=request.Carbohydrates;
                nutrient.Fat=request.Fat;

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.NutrientRepository.Update(nutrient);

                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();

                Log.Information("Nutrient updated successfully: ID={NutrientId}", request.NutrientId);

                return _mapper.Map<NutrientDto>(nutrient);
            }
            catch(NotFoundException ex)
            {
                Log.Error(ex, "Nutriebt not found");
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update nutrient");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Fail update exercise", ex);
            }

        }
    }
}


