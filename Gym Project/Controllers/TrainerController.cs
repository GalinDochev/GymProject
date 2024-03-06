using Gym_Project.Models;
using GymProject.Core.DTOs;
using GymProject.Infrastructure.Data.Models;
using GymProject.Core.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;

namespace Gym_Project.Controllers
{
    public class TrainerController : Controller
    {
        private TrainersService _trainerService;
        private ExerciseService _exerciseService;
        public TrainerController(TrainersService trainerService, ExerciseService exerciseService)
        {
            _trainerService = trainerService;
            _exerciseService = exerciseService;
        }
        public async Task<IActionResult> Index()
        {
            var trainersDTO = await _trainerService.GetAllNotDeletedTrainers();
            var trainers = trainersDTO.Select(t => new AllTrainersViewModel
            {
                FullName = t.FullName,
                Age = t.Age,
                Id = t.Id,
                ImageUrl = t.ImageUrl,
                Slogan = t.Slogan
            }
            ).ToList();
            return View(trainers);
        }

        public async Task<IActionResult> ViewProfile(int Id)
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

        [HttpGet]
        public async Task<IActionResult> AddTrainer()
        {
            var trainerDTO = await _trainerService.GetTrainerView();
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

        [HttpPost]
        public async Task<IActionResult> AddTrainer(AddTrainerViewModel trainer)
        {
            if (!ModelState.IsValid)
            {
                trainer.Exercises = await _exerciseService.GetAllNotDeletedExForTrainers();

                return View(trainer);
            }
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
        [HttpGet]
        public async Task <IActionResult> EditTrainer (int Id)
        {
            var trainerDTO=await _trainerService.GetTrainerByIdForEdit(Id);
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

        [HttpPost]
        public async Task<IActionResult> EditTrainer(AddTrainerViewModel trainerViewModel, int Id)
        {
            if (!ModelState.IsValid)
            {
                trainerViewModel.Exercises = await _exerciseService.GetAllNotDeletedExForTrainers();

                return View(trainerViewModel);
            }
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

        public async Task<IActionResult> DeleteTrainer (int Id)
        {
            await _trainerService.DeleteTrainer(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
