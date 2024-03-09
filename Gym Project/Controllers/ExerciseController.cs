using Gym_Project.Models;
using Gym_Project.Models.ExerciseModels;
using GymProject.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Project.Controllers
{
    public class ExerciseController : Controller
    {
        private ExerciseService _exerciseService;
        public ExerciseController(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }
        public async Task<IActionResult> Index()
        {
            var exercisesDTOs = await _exerciseService.GetAllNotDeletedExercises();
            var exercises=exercisesDTOs.Select(e=>new AllExercisesViewModel
            {
                Id = e.Id,
                Name = e.Name,
                DifficultyLevel = e.DifficultyLevel,
                ImageUrl = e.ImageUrl,
                Repetitions = e.Repetitions,
                Sets = e.Sets,
                MuscleGroups = string.Join(", ", e.MuscleGroups)
            }).ToList();

            return View(exercises);
        }
    }
}
