using Gym_Project.Models.ExerciseModels;
using Gym_Project.Models.TrainerModels;
using Gym_Project.Models.WorkoutsModels;
using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var userId = GetUserId();
            var workoutsDTOs = await _workoutService.GetAllNotDeletedWorkouts();
            var workouts = workoutsDTOs.Select(e => new AllWorkoutsViewModel
            {
                Id = e.Id,
                Name = e.Name,
                DifficultyLevel = e.DifficultyLevel,
                ImageUrl = e.ImageUrl,
                Category = e.Category.Name,
                Duration = e.Duration,
                CreatorId = e.Creator.Id,
                CreatorName = e.Creator.UserName,
                IsJoined = e.UserWorkouts.Any(uw => uw.UserId == userId &&
                                                     uw.UserId != e.Creator.Id &&
                                                     !uw.IsDeleted)
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

        public async Task<IActionResult> JoinWorkout(int Id)
        {
            var userId = GetUserId();
            await _workoutService.JoinWorkout(Id, GetUserId());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LeaveWorkout(int Id)
        {
            var userId = GetUserId();

            await _workoutService.RemoveWorkoutFromCollectionAsync(Id, userId);

            return RedirectToAction(nameof(Index));
        }


        protected string GetUserId()
        {
            string id = string.Empty;

            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return id;
        }
    }
}
