using Gym_Project.Models.TrainerModels;
using GymProject.Common.Constants;
using GymProject.Core.DTOs.TrainerDTOs;
using GymProject.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Project.Controllers
{
    [Authorize]
    public class TrainerController : Controller
    {
        private TrainersService _trainerService;
        private ExerciseService _exerciseService;
        private ILogger<TrainerController> _logger;
        public TrainerController(TrainersService trainerService, ExerciseService exerciseService,ILogger<TrainerController> logger)
        {
            _trainerService = trainerService;
            _exerciseService = exerciseService;
            _logger = logger;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string searchString,
            int pageNumber = PaginationConstants.PageNumber, 
            int pageSize = PaginationConstants.PageSize)
        {
            try
            {
                var trainersDTO = await _trainerService.GetAllNotDeletedTrainers();

                if (!string.IsNullOrEmpty(searchString))
                {
                    var lowerCaseSearchString = searchString.ToLower();
                    trainersDTO = trainersDTO.Where(t => t.FullName.ToLower().Contains(lowerCaseSearchString)).ToList();
                }

                var totalRecords = trainersDTO.Count;
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var trainers = trainersDTO
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => new AllTrainersViewModel
                    {
                        FullName = t.FullName,
                        Age = t.Age,
                        Id = t.Id,
                        ImageUrl = t.ImageUrl,
                        Slogan = t.Slogan
                    }).ToList();

                ViewBag.SearchString = searchString ?? string.Empty;
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = pageNumber;

                return View(trainers);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There has been an error in Index/TrainerController ");
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ViewProfile(int Id)
        {
            try
            {
                var trainerDTO = await _trainerService.GetTrainerByIdForProfile(Id);
                var trainer = new TrainerProfileViewModel
                {
                    FullName = trainerDTO.FullName,
                    Age = trainerDTO.Age,
                    Id = trainerDTO.Id,
                    Slogan = trainerDTO.Slogan,
                    Education = trainerDTO.Education,
                    FavouriteExercise = trainerDTO.FavouriteExercise,
                    ImageUrl = trainerDTO.ImageUrl,
                };
                return View(trainer);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There has been an error in Details/TrainerController ");
                return StatusCode(500);
            }
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> AddTrainer()
        {
            try
            {

                var trainerDTO = await _trainerService.GetTrainerDTOModel();
                var trainer = new AddTrainerViewModel
                {
                    FullName = trainerDTO.FullName,
                    Age = trainerDTO.Age,
                    Id = trainerDTO.Id,
                    Slogan = trainerDTO.Slogan,
                    Education = trainerDTO.Education,
                    ImageUrl = trainerDTO.ImageUrl,
                    ExerciseId = trainerDTO.ExerciseId,
                    Exercises = trainerDTO.Exercises
                };
                return View(trainer);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in AddExercise/TrainerController while getting the trainer view model.");
                return StatusCode(500);
                throw;
            }
        }
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddTrainer(AddTrainerViewModel trainer)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    trainer.Exercises = await _exerciseService.GetAllNotDeletedExForTrainers();
                    return View(trainer);
                }
                catch (Exception ex)
                {
                    TempData["ShowException"] = true;
                    TempData["ExceptionMessage"] = "An error occurred while adding the trainer.";
                    return RedirectToAction(nameof(AddTrainer));
                }
            }
            try
            {
                var trainerDTO = new AddTrainerDTO
                {
                    Id = trainer.Id,
                    Age = trainer.Age,
                    Education = trainer.Education,
                    ExerciseId = trainer.ExerciseId,
                    FullName = trainer.FullName,
                    ImageUrl = trainer.ImageUrl,
                    Slogan = trainer.Slogan
                };

                await _trainerService.AddTrainer(trainerDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There has been an error in AddTrainer/TrainerController ");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.AddTrainerErrorMessage}";
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task <IActionResult> EditTrainer (int Id)
        {
            try
            {
                var trainerDTO = await _trainerService.GetTrainerByIdForEdit(Id);
                var trainer = new AddTrainerViewModel
                {
                    FullName = trainerDTO.FullName,
                    Age = trainerDTO.Age,
                    Id = trainerDTO.Id,
                    Slogan = trainerDTO.Slogan,
                    Education = trainerDTO.Education,
                    ImageUrl = trainerDTO.ImageUrl,
                    ExerciseId = trainerDTO.ExerciseId,
                    Exercises = trainerDTO.Exercises
                };
                return View(trainer);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in EditTrainer/TrainerController while getting the trainer view model.");
                return StatusCode(500);
                throw;
            }
        }
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditTrainer(AddTrainerViewModel trainerViewModel, int Id)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                trainerViewModel.Exercises = await _exerciseService.GetAllNotDeletedExForTrainers();

                return View(trainerViewModel);
                }
                catch (Exception ex)
                {
                    TempData["ShowException"] = true;
                    TempData["ExceptionMessage"] = $"{CustomExceptionMessages.EditTrainerErrorMessage}";
                    return RedirectToAction(nameof(EditTrainer));
                }
            }
            try
            {
                var trainerDTO = new AddTrainerDTO
                {
                    Id = trainerViewModel.Id,
                    Age = trainerViewModel.Age,
                    Education = trainerViewModel.Education,
                    ExerciseId = trainerViewModel.ExerciseId,
                    FullName = trainerViewModel.FullName,
                    ImageUrl = trainerViewModel.ImageUrl,
                    Slogan = trainerViewModel.Slogan
                };
                await _trainerService.EditTrainer(trainerDTO);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in EditTrainer/TrainerControllre");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.EditTrainerErrorMessage}"; ;
                return RedirectToAction(nameof(EditTrainer));
            }
        }
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainer (int Id)
        {
            try
            {
                await _trainerService.DeleteTrainer(Id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DeleteTrainer/TrainerControllre");
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = $"{CustomExceptionMessages.DeleteTrainerErrorMessage}"; ;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
