using AutoMapper;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;


namespace LifeStyle.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //exercise
            CreateMap<Exercise, ExerciseDto>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.ExerciseId)
                );
            CreateMap<ExerciseDto, Exercise>();
            CreateMap<CreateExercise, Exercise>();
            CreateMap<UpdateExercise, Exercise>();

            //meal
            CreateMap<Meal, MealDto>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.MealId)
                );
            CreateMap<MealDto, Meal>();

            //nutrient
            CreateMap<Nutrients, NutrientDto>();
            CreateMap<NutrientDto,Nutrients>();

            //planner 
            CreateMap<Planner, PlannerDto>();
            CreateMap<PlannerDto, Planner>();

            //User
            CreateMap<UserProfile, UserDto>()
                 .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.ProfileId)
                );
            CreateMap<UserDto,UserProfile>();


              
        }
    }
}
