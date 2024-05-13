using LifeStyle.Aplication.Interfaces;
using LifeStyle.Aplication.Logic;
using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.Context;
using LifeStyle.Infrastructure.Middleware;
using LifeStyle.Infrastructure.Repository;
using LifeStyle.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRepository<Exercise>, ExerciseRepository>();
builder.Services.AddScoped<IRepository<Nutrients>, NutrientRepository>();
builder.Services.AddScoped<IRepository<Meal>, MealRepository>();
builder.Services.AddScoped<IRepository<UserProfile>, UserRepository>();
builder.Services.AddScoped<IPlannerRepository, PlannerRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IPlannerRepository).Assembly));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("LifeStyleConnection");

builder.Services.AddDbContext<LifeStyleContext>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();


// Middleware
//app.UseMiddleware<RequestProcessingTimeMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

