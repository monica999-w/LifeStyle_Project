using LifeStyle.Aplication.Interfaces;
using LifeStyle.Aplication.Logic;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRepository<Exercise>, ExerciseRepository>();
builder.Services.AddSingleton<IRepository<Meal>, MealRepository>();
builder.Services.AddSingleton<IRepository<UserProfile>, UserRepository>();
builder.Services.AddSingleton<IPlannerRepository, PlannerRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IPlannerRepository).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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

