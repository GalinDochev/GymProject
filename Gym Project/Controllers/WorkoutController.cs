using Gym_Project.Models.ExerciseModels;
using Gym_Project.Models.TrainerModels;
using Gym_Project.Models.WorkoutsModels;
using GymProject.Common.Constants;
using GymProject.Core.DTOs.ExerciseDTOs;
using GymProject.Core.DTOs.WorkoutDTOs;
using GymProject.Core.Exceptions;
using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
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
        private ILogger _logger;

        public WorkoutController(WorkoutService workoutService, ExerciseService exerciseService, ILogger<WorkoutController> logger)
        {
            _workoutService = workoutService;
            _exerciseService = exerciseService;
            _logger = logger;
        }
        [AllowAnonymous]

        public async Task<IActionResult> Index(
            string searchString,
            string category,
            string difficultyLevelGroup,
            int pageNumber = PaginationConstants.PageNumber,
            int pageSize = PaginationConstants.PageSize)
        {
            try
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
                        CreatorId = e.CreatorId,
                        CreatorName = e.Creator.UserName,
                        IsJoined = e.UserWorkouts.Any(uw => uw.UserId == userId &&
                                                         uw.UserId != e.CreatorId &&
                                                         !uw.IsDeleted)
                    }).ToList();
                ViewBag.SearchString = searchString ?? string.Empty;
                ViewBag.PageNumber = pageNumber;
                ViewBag.TotalPages = totalPages;
                return View(workouts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There has been an error in Index/WorkoutController ");
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int Id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "There has been an error in Details/WorkoutController ");
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> JoinWorkout(int Id)
        {
            try
            {
                var userId = GetUserId();
                await _workoutService.JoinWorkout(Id, GetUserId());
                return Redirect("/Identity/Account/Manage/MyWorkouts");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in JoinWorkout/WorkoutController");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.JoinWorkoutErrorMessage}"; ;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> LeaveWorkout(int Id)
        {
            try
            {
                var userId = GetUserId();

                await _workoutService.RemoveWorkoutFromCollectionAsync(Id, userId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in LeaveWorkout/WorkoutController");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.LeaveWorkoutErrorMessage}"; ;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddWorkout()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in AddWorkout/WorkoutController while getting the exercise view model.");
                return StatusCode(500);
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWorkout(AddWorkoutViewModel workoutViewModel)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    workoutViewModel.SelectedExercises = await _exerciseService.GetExercisesNames();
                    workoutViewModel.SelectedCategories = await _workoutService.GetCategoriesNames();

                    return View(workoutViewModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in AddWorkout/WorkoutController");
                    TempData["ShowException"] = true;
                    TempData["ExceptionMessage"] = $"{CustomExceptionMessages.AddWorkoutErrorMessage}"; ;
                    return RedirectToAction(nameof(AddWorkout));
                }
            }
            try
            {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "There has been an error in AddWorkout/WorkoutController ");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.AddWorkoutErrorMessage}";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditWorkout(int Id)
        {
            try
            {
                var workoutDTO = await _workoutService.GetWorkoutByIdForEdit(Id);
                var userId = GetUserId();
                if (userId != workoutDTO.CreatorId)
                {
                    throw new UnAuthorizedActionException("You can't edit a workout that you haven't created");
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
            catch (UnAuthorizedActionException ex)
            {
                _logger.LogError(ex, "An error occurred in EditWorkout/WorkoutControllre");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.UnauthorisedEditWorkoutErrorMessage}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in EditWorkout/WorkoutController while getting the exercise view model.");
                return StatusCode(500);
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWorkout(AddWorkoutViewModel workoutViewModel, int Id)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    workoutViewModel.SelectedExercises = await _exerciseService.GetExercisesNames();
                    workoutViewModel.SelectedCategories = await _workoutService.GetCategoriesNames();

                    return View(workoutViewModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in EditWorkout/WorkoutController");
                    TempData["ShowException"] = true;
                    TempData["ExceptionMessage"] = $"{CustomExceptionMessages.EditWorkoutErrorMessage}"; ;
                    return RedirectToAction(nameof(EditWorkout));
                }
            }
            try
            {
                var userId = GetUserId();
                var exercises = await _exerciseService.GetExercisesByName(workoutViewModel.SelectedExercises);
                var category = await _workoutService.GetCategoryByName(workoutViewModel.Category);
                var workoutDTO = new AddWorkoutDTO
                {
                    Name = workoutViewModel.Name,
                    Id = workoutViewModel.Id,
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

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in EditWorkout/WorkoutControllre");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.EditWorkoutErrorMessage}"; ;
                return RedirectToAction(nameof(EditWorkout));
            }

        }

        public async Task<IActionResult> DeleteWorkout(int Id)
        {
            try
            {
                var userId = GetUserId();
                var workout = await _workoutService.GetWorkoutByIdForEdit(Id);
                await _workoutService.DeleteWorkout(Id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (UnAuthorizedActionException ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteWorkout/WorkoutController");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.UnauthorisedDeleteWorkoutErrorMessage}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteWorkout/WorkoutController");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.DeleteWorkoutErrorMessage}";
                return RedirectToAction(nameof(Index));
            }
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
