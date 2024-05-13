using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Planners.Commands
{
    public record CreatePlanner(int UserId, List<int> MealIds, List<int> ExerciseIds) : IRequest<PlannerDto>;

    public class CreatePlannerHandler : IRequestHandler<CreatePlanner, PlannerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePlannerHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("CreatePlannerHandler instance created.");
        }

        public async Task<PlannerDto> Handle(CreatePlanner request, CancellationToken cancellationToken)
        {
            Log.Information("Handling CreatePlanner command...");
            try
            {
                Log.Information("Creating Planner..");
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                {
                    Log.Warning("Planner with User Id not found: ID={UserId}", request.UserId);
                    throw new NotFoundException($"User with ID {request.UserId} not found");
                }

                var meals = new List<Meal>();
                foreach (var mealId in request.MealIds)
                {
                    var meal = await _unitOfWork.MealRepository.GetById(mealId);
                    if (meal == null)
                    {
                        Log.Warning("Planner with Meal Id not found: ID={MealIds}", request.MealIds);
                        throw new NotFoundException($"Meal with ID {mealId} not found");
                    }
                    meals.Add(meal);
                }

                var exercises = new List<Exercise>();
                foreach (var exerciseId in request.ExerciseIds)
                {
                    var exercise = await _unitOfWork.ExerciseRepository.GetById(exerciseId);
                    if (exercise == null)
                    {
                        Log.Warning("Planner with Exercise Id not found: ID={ExerciseIds}", request.ExerciseIds);
                        throw new NotFoundException($"Exercise with ID {exerciseId} not found");
                    }
                    exercises.Add(exercise);
                }

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                var planner = new Planner(user);

                foreach (var meal in meals)
                {
                    planner.AddMeal(meal);
                }

                foreach (var exercise in exercises)
                {
                    planner.AddExercise(exercise);
                }

                
                await _unitOfWork.PlannerRepository.AddPlanner(planner);
                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("Planner created successfully");

                var plannerDto = _mapper.Map<PlannerDto>(planner);

                return plannerDto;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Not found exception");
                throw ;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create planner");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to create planner", ex);
            }
        }
    }
}

