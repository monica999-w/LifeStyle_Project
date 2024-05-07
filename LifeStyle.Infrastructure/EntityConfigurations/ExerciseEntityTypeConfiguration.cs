using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Models.Planner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace LifeStyle.Infrastructure.EntityConfigurations
{
    public class ExerciseEntityTypeConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.Property(e => e.Type)
               .HasConversion(
                   v => v.ToString(),
                   v => (ExerciseType)Enum.Parse(typeof(ExerciseType), v));
        }
    }
}
