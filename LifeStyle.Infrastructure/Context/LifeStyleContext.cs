using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.EntityConfigurations;
using LifeStyle.Models.Planner;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LifeStyle.Infrastructure.Context
{
    public class LifeStyleContext : IdentityDbContext<IdentityUser>
    {
        public LifeStyleContext(DbContextOptions options) : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; } = default!;
        public DbSet<Exercise> Exercises { get; set; } = default!;
        public DbSet<Nutrients> Nutrients { get; set; } = default!;
        public DbSet<Meal> Meals { get; set; } = default!;
        public DbSet<Planner> Planners { get; set; } = default!;

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfiguration(new PlannerEntityTypeConfiguration());
           modelBuilder.ApplyConfiguration(new ExerciseEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);

        //   modelBuilder.Entity<UserProfile>(b =>
        //{
          
        //    b.HasKey(u => u.Id);
           
        //});

        }
    }
}
