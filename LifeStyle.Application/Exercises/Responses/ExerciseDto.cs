using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;


namespace LifeStyle.Application.Responses
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int DurationInMinutes { get; set; }
        public  ExerciseType Type { get; set; }

        public static ExerciseDto FromExercise(Exercise exercise)
        {
            return new ExerciseDto
            {
                Id = exercise.ExerciseId,
                Name = exercise.Name,
                DurationInMinutes = exercise.DurationInMinutes,
                Type = exercise.Type
            };
        }

        public static Exercise FromExerciseDto(ExerciseDto exerciseDto)
        {
            return new Exercise
            (
                exerciseDto.Id,
                exerciseDto.Name,
                exerciseDto.DurationInMinutes,
                exerciseDto.Type
            );
        }
    }
}
