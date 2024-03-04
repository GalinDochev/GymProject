using Gym_Project.Models;
using GymProject.Core.DTOs;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Project.Controllers
{
    public class TrainerController : Controller
    {
        private TrainersService _service;
        public TrainerController(TrainersService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var trainersDTO = await _service.GetAllNotDeletedTrainers();
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
            var trainerDTO = await _service.GetTrainerByIdForProfile(Id);
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
            var trainerDTO = await _service.GetTrainerView();
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

        
    }
}
