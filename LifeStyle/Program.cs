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
using LifeStyle.Application.Auth;


var builder = WebApplication.CreateBuilder(args);

// Register CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); 
        });
});


var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("LifeStyleConnection");
builder.Services.AddDbContext<LifeStyleContext>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<LifeStyleContext>()
               .AddDefaultTokenProviders();
builder.RegisterAuthentication();


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

builder.Services.AddSwagger();

var app = builder.Build();




app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

//app.UseAuthentication();
app.UseAuthorization();
// Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.MapControllers();

app.Run();

