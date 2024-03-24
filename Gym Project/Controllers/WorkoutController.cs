using Gym_Project.Models.ExerciseModels;
using Gym_Project.Models.TrainerModels;
using Gym_Project.Models.WorkoutsModels;
using GymProject.Common.Constants;
using GymProject.Core.DTOs.ExerciseDTOs;
using GymProject.Core.DTOs.WorkoutDTOs;
using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym_Project.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private WorkoutService _workoutService;
        private ExerciseService _exerciseService;

        public WorkoutController(WorkoutService workoutService, ExerciseService exerciseService)
        {
            _workoutService = workoutService;
            _exerciseService = exerciseService;

        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string searchString,
            string category,
            string difficultyLevelGroup,
            int pageNumber = PaginationConstants.PageNumber,
            int pageSize = PaginationConstants.PageSize)
        {
            var userId = GetUserId();
            var workoutsDTOs = await _workoutService.GetAllNotDeletedWorkouts();

            workoutsDTOs = _workoutService.ApplySearchFilter(workoutsDTOs, searchString);

            workoutsDTOs = _workoutService.ApplyCategoryFilter(workoutsDTOs, category);

            workoutsDTOs = _workoutService.ApplyDifficultyLevelFilter(workoutsDTOs, difficultyLevelGroup);

            var categoryNames = await _workoutService.GetCategoriesNames();
            ViewBag.Categories = categoryNames;


            var totalRecords = workoutsDTOs.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var workouts = workoutsDTOs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new AllWorkoutsViewModel
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
            ViewBag.SearchString = searchString ?? string.Empty;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            return View(workouts);
        }

        [AllowAnonymous]
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
        [HttpGet]
        public async Task<IActionResult> AddWorkout()
        {
            var workoutDTO = await _workoutService.GetWorkoutDTOModel();
            var workout = new AddWorkoutViewModel
            {
                Id = workoutDTO.Id,
                Name = workoutDTO.Name,
                DifficultyLevel = workoutDTO.DifficultyLevel,
                ImageUrl = workoutDTO.ImageUrl,
                Description = workoutDTO.Description,
                Duration = workoutDTO.Duration,
                CreatorId = workoutDTO.CreatorId,
                SelectedCategories = workoutDTO.SelectedCategories.Select(m => m.Name).ToList(),
                SelectedExercises = workoutDTO.SelectedExercises.Select(m => m.Name).ToList()
            };
            return View(workout);
        }
        [HttpPost]
        public async Task<IActionResult> AddWorkout(AddWorkoutViewModel workoutViewModel)
        {
            if (!ModelState.IsValid)
            {
                workoutViewModel.SelectedExercises = await _exerciseService.GetExercisesNames();
                workoutViewModel.SelectedCategories = await _workoutService.GetCategoriesNames();

                return View(workoutViewModel);
            }
            var userId = GetUserId();
            var exercises = await _exerciseService.GetExercisesByName(workoutViewModel.SelectedExercises);
            var category = await _workoutService.GetCategoryByName(workoutViewModel.Category);
            var workoutDTO = new AddWorkoutDTO
            {
                Name = workoutViewModel.Name,
                Description = workoutViewModel.Description,
                DifficultyLevel = workoutViewModel.DifficultyLevel,
                ImageUrl = workoutViewModel.ImageUrl,
                SelectedExercises = exercises,
                Duration = workoutViewModel.Duration,
                CreatorId = userId,
                Category = category
            };

            await _workoutService.AddWorkout(workoutDTO);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditWorkout(int Id)
        {
            var workoutDTO = await _workoutService.GetWorkoutByIdForEdit(Id);
            var userId = GetUserId();
            if (userId!=workoutDTO.CreatorId)
            {
                throw new InvalidOperationException("You cant edit a workout that you havent created");
            }
            var workoutViewModel = new AddWorkoutViewModel
            {
                Name = workoutDTO.Name,
                Id = workoutDTO.Id,
                Duration = workoutDTO.Duration,
                CreatorId = workoutDTO.CreatorId,
                Description = workoutDTO.Description,
                DifficultyLevel = workoutDTO.DifficultyLevel,
                ImageUrl = workoutDTO.ImageUrl,
                SelectedExercises = workoutDTO.SelectedExercises.Select(m => m.Name).ToList(),
                SelectedCategories = workoutDTO.SelectedCategories.Select(m => m.Name).ToList()
            };
            return View(workoutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditWorkout(AddWorkoutViewModel workoutViewModel, int Id)
        {
            if (!ModelState.IsValid)
            {
                workoutViewModel.SelectedExercises = await _exerciseService.GetExercisesNames();
                workoutViewModel.SelectedCategories = await _workoutService.GetCategoriesNames();

                return View(workoutViewModel);
            }
            var userId = GetUserId();
            var exercises = await _exerciseService.GetExercisesByName(workoutViewModel.SelectedExercises);
            var category = await _workoutService.GetCategoryByName(workoutViewModel.Category);
            var workoutDTO = new AddWorkoutDTO
            {
                Name = workoutViewModel.Name,
                Id=workoutViewModel.Id,
                Description = workoutViewModel.Description,
                DifficultyLevel = workoutViewModel.DifficultyLevel,
                ImageUrl = workoutViewModel.ImageUrl,
                SelectedExercises = exercises,
                Duration = workoutViewModel.Duration,
                CreatorId = userId,
                Category = category
            };
            await _workoutService.EditWorkout(workoutDTO);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteWorkout(int Id)
        {
            await _workoutService.DeleteWorkout(Id);
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
