using GymProject.Core.DTOs.ExerciseDTOs;
using GymProject.Core.DTOs.TrainerDTOs;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymProject.Core.Services
{
    public class ExerciseService
    {
        private ExerciseRepository exerciseRepository;
        public ExerciseService(Repository<Exercise> exerciseRepo)
        {
            exerciseRepository = (ExerciseRepository)exerciseRepo;
        }

        public async Task<List<ExerciseForTrainerDTO>> GetAllNotDeletedExForTrainers()
        {
            var exercises = await exerciseRepository.GetAllNotDeleted();
            var exercisesDTOs = exercises.Select(e => new ExerciseForTrainerDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            return exercisesDTOs;
        }


        public async Task<List<ExerciseCardDTO>> GetAllNotDeletedExercises()
        {
            var exercises = await exerciseRepository.GetAllNotDeleted();
            var exercisesDTOs = exercises.Select(e => new ExerciseCardDTO
            {
                Id = e.Id,
                Name = e.Name,
                DifficultyLevel = e.DifficultyLevel,
                ImageUrl = e.ImageUrl,
                Repetitions = e.Repetitions,
                Sets = e.Sets,
                IsDeleted = e.IsDeleted,
                DeleteTime = e.DeleteTime,
                MuscleGroups = e.ExerciseMuscleGroups.Select(emg => emg.MuscleGroup.Name).ToList()
            }
            ).ToList();
            return exercisesDTOs;
        }

        public async Task<ExerciseDetailsDTO> GetExerciseByIdForDetails(int Id)
        {
            var exercise = await exerciseRepository.GetById(Id);

            var exerciseDTO = new ExerciseDetailsDTO
            {
                Name = exercise.Name,
                Id = exercise.Id,
                Description=exercise.Description,
                DifficultyLevel = exercise.DifficultyLevel,
                ImageUrl = exercise.ImageUrl,
                MuscleGroups = exercise.ExerciseMuscleGroups.Select(emg => emg.MuscleGroup.Name).ToList(),
                Repetitions = exercise.Repetitions,
                Sets = exercise.Sets,
            };
            return exerciseDTO;
        }
    }
}
