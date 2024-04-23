﻿using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Exercises.Query
{
    public record GetAllExercise : IRequest<List<ExerciseDto>>;
    public class GetAllExercisesHandler : IRequestHandler<GetAllExercise, List<ExerciseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllExercisesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ExerciseDto>> Handle(GetAllExercise request, CancellationToken cancellationToken)
        {
            var exercise = await _unitOfWork.ExerciseRepository.GetAll(); 
            return exercise.Select(ExerciseDto.FromExercise).ToList();
        }
    }


}
