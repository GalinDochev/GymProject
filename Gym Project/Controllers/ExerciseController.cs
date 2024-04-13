using Gym_Project.Models;
using Gym_Project.Models.ExerciseModels;
using Gym_Project.Models.TrainerModels;
using GymProject.Common.Constants;
using GymProject.Core.DTOs.ExerciseDTOs;
using GymProject.Core.DTOs.TrainerDTOs;
using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Project.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        private ExerciseService _exerciseService;
        private ILogger<ExerciseController> _logger;
        public ExerciseController(ExerciseService exerciseService,ILogger<ExerciseController> logger)
        {
            _exerciseService = exerciseService;
            _logger = logger;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index
            (string searchString,
            int pageNumber = PaginationConstants.PageNumber,
            int pageSize = PaginationConstants.PageSize)

        {
            try
            {
                var exercisesDTOs = await _exerciseService.GetAllNotDeletedExercises();
                if (!string.IsNullOrEmpty(searchString))
                {
                    var lowerCaseSearchString = searchString.ToLower();
                    exercisesDTOs = exercisesDTOs.Where(e => e.Name.ToLower().Contains(lowerCaseSearchString)).ToList();
                }
                var totalRecords = exercisesDTOs.Count;
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var exercises = exercisesDTOs
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new AllExercisesViewModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DifficultyLevel = e.DifficultyLevel,
                        ImageUrl = e.ImageUrl,
                        Repetitions = e.Repetitions,
                        Sets = e.Sets,
                        MuscleGroups = string.Join(", ", e.MuscleGroups)
                    }).ToList();

                ViewBag.SearchString = searchString ?? string.Empty;
                ViewBag.PageNumber = pageNumber;
                ViewBag.TotalPages = totalPages;
                return View(exercises);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There has been an error in Index/ExerciseController ");
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int Id)
        {
            try
            {
                var exerciseDTO = await _exerciseService.GetExerciseByIdForDetails(Id);
                var exercise = new ExerciseDetailsViewModel
                {
                    Id = exerciseDTO.Id,
                    Name = exerciseDTO.Name,
                    DifficultyLevel = exerciseDTO.DifficultyLevel,
                    ImageUrl = exerciseDTO.ImageUrl,
                    Repetitions = exerciseDTO.Repetitions,
                    Sets = exerciseDTO.Sets,
                    MuscleGroups = string.Join(", ", exerciseDTO.MuscleGroups),
                    Description = exerciseDTO.Description
                };
                return View(exercise);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There has been an error in Details/ExerciseController ");
                return StatusCode(500);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddExercise()
        {
            try
            {
                var exerciseDTO = await _exerciseService.GetExerciseDTOModel();
                var exercise = new AddExerciseViewModel
                {
                    Id = exerciseDTO.Id,
                    Name = exerciseDTO.Name,
                    DifficultyLevel = exerciseDTO.DifficultyLevel,
                    ImageUrl = exerciseDTO.ImageUrl,
                    Repetitions = exerciseDTO.Repetitions,
                    Sets = exerciseDTO.Sets,
                    SelectedMuscleGroups = exerciseDTO.SelectedMuslceGroups.Select(m => m.Name).ToList(),
                    Description = exerciseDTO.Description,
                };
                return View(exercise);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in AddExercise/ExerciseController while getting the exercise view model.");
                return StatusCode(500);
                throw;
            }
        }
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddExercise(AddExerciseViewModel exerciseViewModel)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    exerciseViewModel.SelectedMuscleGroups = await _exerciseService.GetMuscleGroupsNames();

                    return View(exerciseViewModel);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in AddExercise/ExerciseControllre");
                    TempData["ShowException"] = true;
                    TempData["ExceptionMessage"] = $"{CustomExceptionMessages.AddExerciseErrorMessage}"; ;
                    return RedirectToAction(nameof(AddExercise));
                }
            }
            try
            {
                var muscleGroups = await _exerciseService.GetMuscleGroupsByName(exerciseViewModel.SelectedMuscleGroups);
                var exerciseDTO = new AddExerciseDTO
                {
                    Id = exerciseViewModel.Id,
                    Name = exerciseViewModel.Name,
                    Description = exerciseViewModel.Description,
                    DifficultyLevel = exerciseViewModel.DifficultyLevel,
                    ImageUrl = exerciseViewModel.ImageUrl,
                    Repetitions = exerciseViewModel.Repetitions,
                    Sets = exerciseViewModel.Sets,
                    SelectedMuslceGroups = muscleGroups
                };

                await _exerciseService.AddExercise(exerciseDTO);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There has been an error in AddExercise/ExerciseController ");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] =$"{CustomExceptionMessages.AddExerciseErrorMessage}";
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditExercise(int Id)
        {
            try
            {
                var exerciseDTO = await _exerciseService.GetExerciseByIdForEdit(Id);
                var exercise = new AddExerciseViewModel
                {
                    Id = exerciseDTO.Id,
                    Name = exerciseDTO.Name,
                    Description = exerciseDTO.Description,
                    DifficultyLevel = exerciseDTO.DifficultyLevel,
                    ImageUrl = exerciseDTO.ImageUrl,
                    Repetitions = exerciseDTO.Repetitions,
                    Sets = exerciseDTO.Sets,
                    SelectedMuscleGroups = exerciseDTO.SelectedMuslceGroups.Select(m => m.Name).ToList(),
                };
                return View(exercise);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in EditExercise/ExerciseController while getting the exercise view model.");
                return StatusCode(500);
                throw;
            }
        }
        [Authorize(Roles ="Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditExercise(AddExerciseViewModel exerciseViewModel, int Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    exerciseViewModel.SelectedMuscleGroups = await _exerciseService.GetMuscleGroupsNames();

                    return View(exerciseViewModel);
                }
                var muscleGroups = await _exerciseService.GetMuscleGroupsByName(exerciseViewModel.SelectedMuscleGroups);
                var exerciseDTO = new AddExerciseDTO
                {
                    Id = exerciseViewModel.Id,
                    Name = exerciseViewModel.Name,
                    Description = exerciseViewModel.Description,
                    DifficultyLevel = exerciseViewModel.DifficultyLevel,
                    ImageUrl = exerciseViewModel.ImageUrl,
                    Repetitions = exerciseViewModel.Repetitions,
                    Sets = exerciseViewModel.Sets,
                    SelectedMuslceGroups = muscleGroups
                };
                await _exerciseService.EditExercise(exerciseDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in EditExercise/ExerciseControllre");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.EditExerciseErrorMessage}"; ;
                return RedirectToAction(nameof(EditExercise));
            }
        }
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExercise(int Id)
        {
            try
            {
                await _exerciseService.DeleteExercise(Id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteExercise/ExerciseControllre");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.EditExerciseErrorMessage}"; ;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
