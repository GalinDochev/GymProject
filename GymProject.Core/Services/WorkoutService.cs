using GymProject.Core.DTOs.ExerciseDTOs;
using GymProject.Core.DTOs.WorkoutDTOs;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.Services
{
    public class WorkoutService
    {
        private WorkoutRepository workoutRepository;
        public WorkoutService(Repository<Workout> workoutRepo)
        {
            workoutRepository = (WorkoutRepository)workoutRepo;
        }


        public async Task<List<WorkoutCardDTO>> GetAllNotDeletedWorkouts()
        {
            var workouts = await workoutRepository.GetAllNotDeleted();
            var workoutsDTOs = workouts.Select(e => new WorkoutCardDTO
            {
                Id = e.Id,
                Name = e.Name,
                DifficultyLevel = e.DifficultyLevel,
                ImageUrl = e.ImageUrl,
                CategoryId = e.CategoryId,
                Category = e.Category,
                CreatorId = e.CreatorId,
                Creator = e.Creator,
                Duration = e.Duration
            }
            ).ToList();
            return workoutsDTOs;
        }

        public async Task<WorkoutDetailsDTO> GetWorkoutByIdForDetails(int Id)
        {
            var workout = await workoutRepository.GetById(Id);

            var workoutDTO = new WorkoutDetailsDTO
            {
                Name = workout.Name,
                Id = workout.Id,
                Description = workout.Description,
                DifficultyLevel = workout.DifficultyLevel,
                ImageUrl = workout.ImageUrl,
                Creator = workout.Creator,
                Category = workout.Category,
                CreatorId = workout.CreatorId,
                CategoryId = workout.CategoryId,
                Duration = workout.Duration,
                ExerciseWorkouts = workout.ExerciseWorkouts,
            };
            return workoutDTO;
        }
    }
}
