using LifeStyle.Models.Planner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeStyle.Infrastructure.EntityConfigurations
{
    public class PlannerEntityTypeConfiguration : IEntityTypeConfiguration<Planner>
    {
        public void Configure(EntityTypeBuilder<Planner> builder)
        {
          builder
                .HasMany(p => p.Meals)
                .WithMany()
                .UsingEntity(j => j.ToTable("PlannerMeal"));

          builder
               .HasMany(p => p.Exercises)
               .WithMany()
               .UsingEntity(j => j.ToTable("PlannerExercise"));
        }
    }
}
