

using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.EntityConfigurations;
using LifeStyle.Models.Planner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LifeStyle.Infrastructure.Context
{
    public class LifeStyleContext : DbContext
    {

        public DbSet<UserProfile> UserProfiles { get; set; } = default!;
        public DbSet<Exercise> Exercises { get; set; } = default!;
        public DbSet<Nutrients> Nutrients { get; set; } = default!;
        public DbSet<Meal> Meals { get; set; } = default!;
        public DbSet<Planner> Planners { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server = (localdb)\projectmodels; Database = LifeStyle;Trusted_Connection=True;")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Name },
                       LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlannerEntityTypeConfiguration());

        }
    }
}
