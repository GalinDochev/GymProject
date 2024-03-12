﻿using Gym_Project.Models;
using Gym_Project.Models.ExerciseModels;
using Gym_Project.Models.TrainerModels;
using GymProject.Core.DTOs.ExerciseDTOs;
using GymProject.Core.DTOs.TrainerDTOs;
using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
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
            var exercises = exercisesDTOs.Select(e => new AllExercisesViewModel
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

        public async Task<IActionResult> Details(int Id)
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

        [HttpGet]
        public async Task<IActionResult> AddExercise()
        {
            var exerciseDTO = await _exerciseService.GetExerciseViewModel();
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

        [HttpPost]
        public async Task<IActionResult> AddExercise(AddExerciseViewModel exerciseViewModel)
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

            await _exerciseService.AddExercise(exerciseDTO);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> EditExercise(int Id)
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

        [HttpPost]
        public async Task<IActionResult> EditExercise(AddExerciseViewModel exerciseViewModel, int Id)
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
    }
}