using AutoMapper;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Paged;
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


            // meal
            CreateMap<Meal, MealDto>()
              .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image));
            CreateMap<MealDto, Meal>();



            //nutrient
            CreateMap<Nutrients, Nutrient>();
            CreateMap<Nutrient,Nutrients>();

            //planner 
            CreateMap<Planner, PlannerDto>()
            .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile != null ? src.Profile.ProfileId : 0))
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

        }
    }
}
