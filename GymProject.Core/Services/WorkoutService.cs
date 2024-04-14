using GymProject.Core.DTOs.ExerciseDTOs;
using GymProject.Core.DTOs.WorkoutDTOs;
using GymProject.Core.Exceptions;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private ExerciseWorkoutRepository exerciseWorkoutRepository;
        private ILogger<WorkoutService> _logger;
        public WorkoutService
            (Repository<Workout> workoutRepo,
            Repository<UserWorkout> userWorkoutRepo,
            Repository<Exercise> exerciseRepo,
            Repository<Category> categoryRepo,
            Repository<ExerciseWorkout> exerciseWorkoutRepo,
            ILogger<WorkoutService> logger)
        {
            workoutRepository = (WorkoutRepository)workoutRepo;
            userWorkoutRepository = (UserWorkoutRepository)userWorkoutRepo;
            exerciseRepository = (ExerciseRepository)exerciseRepo;
            categoryRepository = (CategoryRepository)categoryRepo;
            exerciseWorkoutRepository = (ExerciseWorkoutRepository)exerciseWorkoutRepo;
            _logger = logger;
        }


        public async Task<List<WorkoutCardDTO>> GetAllNotDeletedWorkouts()
        {
            try
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
                }).ToList();
                return workoutsDTOs;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred in GetAllNotDeletedWorkouts method.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllNotDeletedWorkouts method.");
                throw;
            }
        }

        public async Task<WorkoutDetailsDTO> GetWorkoutByIdForDetails(int Id)
        {
            try
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
                    ExerciseWorkouts = workout.ExerciseWorkouts.Where(ew => ew.IsDeleted == false).ToList(),
                };
                return workoutDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting workout details for Workout with Id: {Id}");

                throw;
            }
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
            try
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
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred while getting the workout DTO model.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the workout DTO model.");
                throw;
            }
        }

        public async Task<Category> GetCategoryByName(string categoryName)
        {
            try
            {
                var category = await categoryRepository.GetCategoryByName(categoryName);
                return category;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred in GetCategoryByName method.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCategoryByName method.");
                throw;
            }

        }

        public async Task<List<string>> GetCategoriesNames()
        {
            try
            {
                var categories = await categoryRepository.GetAllNotDeleted();
                var categoriesNames = categories.Select(m => m.Name).ToList();
                return categoriesNames;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred in GetCategoriesNames method.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCategoriesNames method.");
                throw;
            }
        }

        public async Task AddWorkout(AddWorkoutDTO workoutDTO)
        {
            try
            {
                var workoutToAdd = new Workout
                {
                    Id = workoutDTO.Id,
                    Description = workoutDTO.Description,
                    DifficultyLevel = workoutDTO.DifficultyLevel,
                    Name = workoutDTO.Name,
                    ImageUrl = workoutDTO.ImageUrl,
                    Duration = workoutDTO.Duration,
                    Category = workoutDTO.Category,
                    CreatorId = workoutDTO.CreatorId,
                    CategoryId = workoutDTO.Category.Id
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
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred during the database update when adding a workout.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation occurred when adding a workout.");
                throw;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An SQL exception occurred when adding a workout.");
                throw;
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "A timeout exception occurred when adding a workout.");
                throw;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "A not supported exception occurred when adding a workout.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when adding a workout.");
                throw;
            }
        }

        public async Task<AddWorkoutDTO> GetWorkoutByIdForEdit(int id)
        {
            try
            {
                var workout = await workoutRepository.GetById(id);
                var selectedExercises = await exerciseRepository.GetAllNotDeleted();
                var selectedCategories = await categoryRepository.GetAllNotDeleted();
                var workoutDTO = new AddWorkoutDTO
                {
                    Name = workout.Name,
                    Id = workout.Id,
                    CreatorId = workout.CreatorId,
                    Description = workout.Description,
                    DifficultyLevel = workout.DifficultyLevel,
                    ImageUrl = workout.ImageUrl,
                    Duration = workout.Duration,
                    SelectedExercises = selectedExercises.ToList(),
                    SelectedCategories = selectedCategories.ToList(),
                };
                return workoutDTO;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An SQL exception occurred when getting a workout for edit.");
                throw;
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "A timeout exception occurred when getting a workout for edit.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when getting a workout for edit.");
                throw;
            }
        }

        public async Task EditWorkout(AddWorkoutDTO workoutDTO)
        {
            try
            {
                var workoutToEdit = await workoutRepository.GetById(workoutDTO.Id);
                if (workoutToEdit.CreatorId != workoutDTO.CreatorId)
                {
                    throw new Exception("You cannot edit a workout that you havent created");
                }
                workoutToEdit.Name = workoutDTO.Name;
                workoutToEdit.Id = workoutDTO.Id;
                workoutToEdit.DifficultyLevel = workoutDTO.DifficultyLevel;
                workoutToEdit.Description = workoutDTO.Description;
                workoutToEdit.ImageUrl = workoutDTO.ImageUrl;
                workoutToEdit.Duration = workoutDTO.Duration;
                workoutToEdit.Category = workoutDTO.Category;
                workoutToEdit.CategoryId = workoutDTO.Category.Id;
                var existingExercises = workoutToEdit.ExerciseWorkouts.ToList();

                foreach (var existingExercise in existingExercises)
                {
                    if (!workoutDTO.SelectedExercises.Any(mg => mg.Id == existingExercise.ExerciseId))
                    {
                        if (!existingExercise.IsDeleted)
                        {
                            await exerciseWorkoutRepository.Delete(existingExercise);
                        }
                    }
                }

                foreach (var exercise in workoutDTO.SelectedExercises)
                {
                    var existingAssociation = existingExercises.FirstOrDefault(emg => emg.ExerciseId == exercise.Id);

                    if (existingAssociation == null)
                    {
                        var exerciseWorkout = new ExerciseWorkout
                        {
                            WorkoutId = workoutToEdit.Id,
                            ExerciseId = exercise.Id
                        };

                        workoutToEdit.ExerciseWorkouts.Add(exerciseWorkout);

                        await exerciseWorkoutRepository.Add(exerciseWorkout);
                    }
                    else if (existingAssociation.IsDeleted)
                    {

                        existingAssociation.IsDeleted = false;
                        existingAssociation.DeleteTime = default;
                        await exerciseWorkoutRepository.Update(existingAssociation);
                    }
                }
                await workoutRepository.Update(workoutToEdit);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while editing workout.");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while editing workout.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation occurred while editing workout.");
                throw;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Operation not supported error occurred while editing workout.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while editing workout.");
                throw;
            }
        }

        public async Task DeleteWorkout(int Id, string userId)
        {
            try
            {
                var workoutToDelete = await workoutRepository.GetById(Id);
                if (workoutToDelete.CreatorId != userId)
                {
                    throw new UnAuthorizedActionException("You cant Delete a workout that you havent created");
                }
                foreach (var exerciseWorkout in workoutToDelete.ExerciseWorkouts)
                {
                    await exerciseWorkoutRepository.Delete(exerciseWorkout);
                }
                foreach (var userWorkout in workoutToDelete.UsersWorkouts)
                {
                    await userWorkoutRepository.Delete(userWorkout);
                }

                await workoutRepository.Delete(workoutToDelete);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while deleting workout.");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while deleting workout.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation occurred while deleting workout.");
                throw;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Operation not supported error occurred while deleting workout.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting workout.");
                throw;
            }
        }

        public async Task<List<WorkoutCardDTO>> GetAllNotDeletedWorkoutsForUser(string userId)
        {
            var allWorkouts = await workoutRepository.GetAllNotDeleted();
            var workouts = allWorkouts.Where(w => w.UsersWorkouts.Any(uw => uw.UserId == userId && uw.IsDeleted == false)).ToList().
                Select(w => new WorkoutCardDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    UserWorkouts = w.UsersWorkouts,
                    Category = w.Category,
                    Creator = w.Creator,
                    CategoryId = w.CategoryId,
                    CreatorId = w.CreatorId,
                    DifficultyLevel = w.DifficultyLevel,
                    Duration = w.Duration,
                    ImageUrl = w.ImageUrl
                }).ToList();
            return workouts;
        }
        public List<WorkoutCardDTO> ApplySearchFilter(List<WorkoutCardDTO> workoutsDTOs, string searchString)
        {
            try
            {
                if (workoutsDTOs == null)
                    throw new ArgumentNullException(nameof(workoutsDTOs));

                if (string.IsNullOrEmpty(searchString))
                    return workoutsDTOs;

                var lowerCaseSearchString = searchString.ToLower();
                return workoutsDTOs.Where(w => w.Name?.ToLower().Contains(lowerCaseSearchString) ?? false).ToList();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Argument was null.");
                throw;
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Null reference encountered.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation occurred.");
                throw;
            }
        }

        public List<WorkoutCardDTO> ApplyCategoryFilter(List<WorkoutCardDTO> workoutsDTOs, string category)
        {
            try
            {
                if (workoutsDTOs == null)
                    throw new ArgumentNullException(nameof(workoutsDTOs));

                if (string.IsNullOrEmpty(category))
                    return workoutsDTOs;

                return workoutsDTOs.Where(w => w.Category?.Name == category).ToList();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Argument was null.");
                throw;
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Null reference encountered.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation occurred.");
                throw;
            }
        }

        public List<WorkoutCardDTO> ApplyDifficultyLevelFilter(List<WorkoutCardDTO> workoutsDTOs, string difficultyLevelGroup)
        {
            try
            {
                if (workoutsDTOs == null)
                    throw new ArgumentNullException(nameof(workoutsDTOs));

                if (string.IsNullOrEmpty(difficultyLevelGroup))
                    return workoutsDTOs;

                switch (difficultyLevelGroup)
                {
                    case "1-3":
                        return workoutsDTOs.Where(w => w.DifficultyLevel >= 1 && w.DifficultyLevel <= 3).ToList();
                    case "4-7":
                        return workoutsDTOs.Where(w => w.DifficultyLevel >= 4 && w.DifficultyLevel <= 7).ToList();
                    case "8-10":
                        return workoutsDTOs.Where(w => w.DifficultyLevel >= 8 && w.DifficultyLevel <= 10).ToList();
                    default:
                        return workoutsDTOs;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Argument was null.");
                throw;
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Null reference encountered.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation occurred.");
                throw;
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Format exception occurred.");
                throw;
            }
        }
    }
}
