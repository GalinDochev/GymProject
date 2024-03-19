using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationDContext(builder.Configuration);
builder.Services.AddApplicationIdentity(builder.Configuration);
builder.Services.AddScoped<Repository<Trainer>, TrainersRepository>();
builder.Services.AddScoped<Repository<Exercise>, ExerciseRepository>();
builder.Services.AddScoped<Repository<ExerciseMuscleGroup>, ExerciseMuscleGroupRepository>();
builder.Services.AddScoped<Repository<MuscleGroup>,MuscleGroupRepository>();
builder.Services.AddScoped<Repository<Workout>,WorkoutRepository>();
builder.Services.AddScoped<Repository<UserWorkout>,UserWorkoutRepository>();
builder.Services.AddScoped<Repository<Category>,CategoryRepository>();
builder.Services.AddScoped<Repository<ExerciseWorkout>,ExerciseWorkoutRepository>();
builder.Services.AddScoped<TrainersService>();
builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<WorkoutService>();
builder.Services.AddControllersWithViews();

builder.Services.AddApplicationServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
