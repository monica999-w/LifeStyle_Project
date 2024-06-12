using LifeStyle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Exercises.Responses
{
    public class ExerciseFilterDto
    {
        public ExerciseType? Type { get; set; }
        public Equipment? Equipment { get; set; }
        public MajorMuscle? MajorMuscle { get; set; }
    }

}
