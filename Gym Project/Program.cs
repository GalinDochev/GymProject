using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddApplicationDContext(builder.Configuration);
builder.Services.AddApplicationIdentity(builder.Configuration);
builder.Services.AddScoped<Repository<Trainer>, TrainersRepository>();
builder.Services.AddScoped<Repository<Exercise>, ExerciseRepository>();
builder.Services.AddScoped<Repository<ExerciseMuscleGroup>, ExerciseMuscleGroupRepository>();
builder.Services.AddScoped<Repository<MuscleGroup>, MuscleGroupRepository>();
builder.Services.AddScoped<Repository<Workout>, WorkoutRepository>();
builder.Services.AddScoped<Repository<UserWorkout>, UserWorkoutRepository>();
builder.Services.AddScoped<Repository<Category>, CategoryRepository>();
builder.Services.AddScoped<Repository<ExerciseWorkout>, ExerciseWorkoutRepository>();
builder.Services.AddScoped<TrainersService>();
builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<WorkoutService>();
builder.Services.AddControllersWithViews(options =>
options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>());
builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
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

app.UseEndpoints(endPoints =>
    {
        app.MapDefaultControllerRoute();
        app.MapRazorPages();
    }
    );
app.UseStatusCodePages(context =>
{
    var statusCode = context.HttpContext.Response.StatusCode;
    if (statusCode == StatusCodes.Status404NotFound)
    {
        context.HttpContext.Response.Redirect($"/Error/NotFound");
    }
    else if (statusCode == StatusCodes.Status500InternalServerError)
    {
        context.HttpContext.Response.Redirect($"/Error/InternalError");
    }
    return Task.CompletedTask;
});
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string email = "galindochev953@gmail.com";
    var password = "galindochev953Pass";
    var existingUser = await userManager.FindByEmailAsync(email);
    if (existingUser == null || !string.Equals(existingUser.Email, email, StringComparison.OrdinalIgnoreCase))
    {
        var user = new ApplicationUser();
        user.UserName = email;
        user.Email = email;
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }
}
app.Run();
