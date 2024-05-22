using LifeStyle.Aplication.Interfaces;
using LifeStyle.Aplication.Logic;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Services;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.Context;
using LifeStyle.Infrastructure.Middleware;
using LifeStyle.Infrastructure.Repository;
using LifeStyle.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LifeStyle.Application.Extensions;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.RegisterAuthentication();
builder.Services.AddSwagger();

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("LifeStyleConnection");
builder.Services.AddDbContext<LifeStyleContext>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<LifeStyleContext>()
               .AddDefaultTokenProviders();

builder.Services.AddScoped<IRepository<Exercise>, ExerciseRepository>();
builder.Services.AddScoped<IRepository<Nutrients>, NutrientRepository>();
builder.Services.AddScoped<IRepository<Meal>, MealRepository>();
builder.Services.AddScoped<IRepository<UserProfile>, UserRepository>();
builder.Services.AddScoped<IPlannerRepository, PlannerRepository>();
builder.Services.AddScoped<IdentityService>();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateExercise).Assembly));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

//// Configurarea Identity
//builder.Services.AddIdentityCore<UserProfile>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequiredLength = 5;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//})
//.AddRoles<IdentityRole>()
//.AddSignInManager<SignInManager<UserProfile>>()
//.AddEntityFrameworkStores<LifeStyleContext>();


var app = builder.Build();


// Middleware
//app.UseMiddleware<ErrorHandlingMiddleware>();


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

