using Gym_Project.Models.ExerciseModels;
using Gym_Project.Models.TrainerModels;
using Gym_Project.Models.WorkoutsModels;
using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Project.Controllers
{
    public class WorkoutController : Controller
    {
        private WorkoutService _workoutService;

        public WorkoutController(WorkoutService workoutService)
        {
            _workoutService = workoutService;
        }
        public async Task<IActionResult> Index()
        {
            var workoutsDTOs = await _workoutService.GetAllNotDeletedWorkouts();
            var workouts = workoutsDTOs.Select(e => new AllWorkoutsViewModel
            {
                Id = e.Id,
                Name = e.Name,
                DifficultyLevel = e.DifficultyLevel,
                ImageUrl = e.ImageUrl,
                Category = e.Category.Name,
                Duration = e.Duration,
                CreatorId = e.Creator.Id
            }).ToList();

            return View(workouts);
        }

        public async Task<IActionResult> Details(int Id)
        {
            var workoutDTO = await _workoutService.GetWorkoutByIdForDetails(Id);
            var workout = new WorkoutDetailsViewModel
            {
                Id = workoutDTO.Id,
                Name = workoutDTO.Name,
                DifficultyLevel = workoutDTO.DifficultyLevel,
                ImageUrl = workoutDTO.ImageUrl,
                Description = workoutDTO.Description,
                CategoryName = workoutDTO.Category.Name,
                Duration = workoutDTO.Duration,
                ExerciseWorkouts = workoutDTO.ExerciseWorkouts,
                CreatorId = workoutDTO.CreatorId,
            };
            return View(workout);
        }
    }
}
