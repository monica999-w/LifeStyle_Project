using AutoMapper;
using LifeStyle.Application.Auth;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Planners.Commands;
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
            CreateMap<Planner, PlannerDto>()
            .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.Profile != null ? src.Profile.ProfileId : 0))
            .ForMember(dest => dest.MealIds, opt => opt.MapFrom(src => src.Meals != null ? src.Meals.Select(m => m.MealId).ToList() : new List<int>()))
            .ForMember(dest => dest.ExerciseIds, opt => opt.MapFrom(src => src.Exercises != null ? src.Exercises.Select(e => e.ExerciseId).ToList() : new List<int>()));
    
            CreateMap<PlannerDto, Planner>();
           

            //User
            CreateMap<UserProfile, UserDto>()
                 .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.ProfileId)
                );
            CreateMap<UserDto,UserProfile>();

            //CreateMap<RegisterDto, UserProfile>()
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            //CreateMap<UserProfile, RegisterDto>();

            //CreateMap<LoginDto, UserProfile>();

        }
    }
}
