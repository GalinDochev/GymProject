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
        private CategoryRepository categoryRepository;
        private UserWorkoutRepository userWorkoutRepository;
        private ExerciseRepository exerciseRepository;
        public WorkoutService(Repository<Workout> workoutRepo, Repository<UserWorkout> userWorkoutRepo, Repository<Exercise> exerciseRepo, Repository<Category> categoryRepo)
        {
            workoutRepository = (WorkoutRepository)workoutRepo;
            userWorkoutRepository = (UserWorkoutRepository)userWorkoutRepo;
            exerciseRepository = (ExerciseRepository)exerciseRepo;
            categoryRepository = (CategoryRepository)categoryRepo;
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
                Duration = e.Duration,
                UserWorkouts = e.UsersWorkouts
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

        public async Task JoinWorkout(int Id, string userId)
        {
            var workout = await workoutRepository.GetById(Id);
            var userWorkout = await userWorkoutRepository.GetByUserIdAndWorkoutId(userId, workout.Id);
            if (workout.CreatorId == userId)
            {
                throw new InvalidOperationException("You cannot join a workout that you created.");
            }

            if (userWorkout != null && userWorkout.IsDeleted == true)
            {
                userWorkout.IsDeleted = false;
                userWorkout.DeleteTime = default;
                workout.UsersWorkouts.Add(userWorkout);
                await userWorkoutRepository.Update(userWorkout);
            }
            else if (userWorkout == null)
            {
                var newUserWorkout = new UserWorkout() { UserId = userId, WorkoutId = workout.Id };
                workout.UsersWorkouts.Add(newUserWorkout);
                await userWorkoutRepository.Add(newUserWorkout);
            }
        }

        public async Task RemoveWorkoutFromCollectionAsync(int Id, string userId)
        {
            var workout = await workoutRepository.GetById(Id);
            if (workout.CreatorId == userId)
            {
                throw new InvalidOperationException("You cannot leave a workout that you created.");
            }
            var userWorkout = await userWorkoutRepository.GetByUserIdAndWorkoutId(userId, workout.Id);
            if (userWorkout != null && userWorkout.IsDeleted == false)
            {
                await userWorkoutRepository.Delete(userWorkout);
                workout.UsersWorkouts.Remove(userWorkout);
            }
            else
            {
                throw new Exception($"Workout with id {Id} is not found in the collection of User with id {userId}");
            }
        }

        public async Task<AddWorkoutDTO> GetWorkoutDTOModel()
        {
            var exercises = await exerciseRepository.GetAllNotDeleted();
            var categories = await categoryRepository.GetAllNotDeleted();

            var model = new AddWorkoutDTO
            {
                SelectedExercises = exercises.ToList(),
                SelectedCategories = categories.ToList()
            };

            return model;
        }

        public async Task<Category> GetCategoryByName(string categoryName)
        {

            var category = await categoryRepository.GetCategoryByName(categoryName);
            return category;
        }

        public async Task<List<string>> GetCategoriesNames()
        {
            var categories = await categoryRepository.GetAllNotDeleted();
            var categoriesNames = categories.Select(m => m.Name).ToList();
            return categoriesNames;
        }

        public async Task AddWorkout(AddWorkoutDTO workoutDTO)
        {
            var workoutToAdd = new Workout
            {
                Id = workoutDTO.Id,
                Description = workoutDTO.Description,
                DifficultyLevel = workoutDTO.DifficultyLevel,
                Name = workoutDTO.Name,
                ImageUrl = workoutDTO.ImageUrl,
                Duration = workoutDTO.Duration,
                Category= workoutDTO.Category,
                CreatorId= workoutDTO.CreatorId,
                CategoryId=workoutDTO.Category.Id
            };
            foreach (var exercise in workoutDTO.SelectedExercises)
            {
                var exerciseWorkout = new ExerciseWorkout
                {
                    ExerciseId = exercise.Id,
                    WorkoutId = workoutDTO.Id
                };

                workoutToAdd.ExerciseWorkouts.Add(exerciseWorkout);
            }
            var userWorkout = new UserWorkout
            {
                UserId = workoutDTO.CreatorId,
                WorkoutId = workoutDTO.Id
            };
            workoutToAdd.UsersWorkouts.Add(userWorkout);
            await workoutRepository.Add(workoutToAdd);
        }
    }
}
