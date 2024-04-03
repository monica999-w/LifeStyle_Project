using LifeStyle.LifeStyle.Aplication.Interfaces;
using LifeStyle.LifeStyle.Aplication.Logic;
using LifeStyle.LifeStyle.Domain.Models.Exercises;
using LifeStyle.LifeStyle.Domain.Models.Meal;
using LifeStyle.LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository<UserProfile>, UserRepository>();
builder.Services.AddScoped<IRepository<Meal>,MealRepository>();
builder.Services.AddScoped<IRepository<Exercise>, ExerciseRepository>();
builder.Services.AddScoped<IPlannerRepository, PlannerRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
