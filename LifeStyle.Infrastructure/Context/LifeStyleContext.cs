﻿using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.EntityConfigurations;
using LifeStyle.Models.Planner;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


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
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);


            // Configurări suplimentare
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.Email)
                .HasPrincipalKey(u => u.Email);

        }
    }
}
