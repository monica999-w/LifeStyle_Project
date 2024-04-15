﻿using LifeStyle.Domain.Enums;


namespace LifeStyle.Domain.Models.Exercises
{
    public class Exercise   
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DurationInMinutes { get; set; }
        public ExerciseType Type { get; set; }

        public Exercise(int id, string name, int durationInMinutes, ExerciseType type)
        {
            this.Id = id;
            Name = name;
            DurationInMinutes = durationInMinutes;
            Type = type;

        }

       
    }
}
