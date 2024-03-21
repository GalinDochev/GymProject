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
        private ExerciseWorkoutRepository exerciseWorkoutRepository;
        public WorkoutService
            (Repository<Workout> workoutRepo,
            Repository<UserWorkout> userWorkoutRepo,
            Repository<Exercise> exerciseRepo,
            Repository<Category> categoryRepo,
            Repository<ExerciseWorkout> exerciseWorkoutRepo)
        {
            workoutRepository = (WorkoutRepository)workoutRepo;
            userWorkoutRepository = (UserWorkoutRepository)userWorkoutRepo;
            exerciseRepository = (ExerciseRepository)exerciseRepo;
            categoryRepository = (CategoryRepository)categoryRepo;
            exerciseWorkoutRepository = (ExerciseWorkoutRepository)exerciseWorkoutRepo;
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
                ExerciseWorkouts = workout.ExerciseWorkouts.Where(ew=>ew.IsDeleted==false).ToList(),
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

        public async Task<AddWorkoutDTO> GetWorkoutByIdForEdit(int id)
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

        public async Task EditWorkout(AddWorkoutDTO workoutDTO)
        {
            var workoutToEdit = await workoutRepository.GetById(workoutDTO.Id);
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

        public async Task DeleteWorkout(int Id)
        {
            var workoutToDelete = await workoutRepository.GetById(Id);
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

        public List<WorkoutCardDTO> ApplySearchFilter(List<WorkoutCardDTO> workoutsDTOs, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return workoutsDTOs;

            var lowerCaseSearchString = searchString.ToLower();
            return workoutsDTOs.Where(w => w.Name.ToLower().Contains(lowerCaseSearchString)).ToList();
        }
        public List<WorkoutCardDTO> ApplyCategoryFilter(List<WorkoutCardDTO> workoutsDTOs, string category)
        {
            if (string.IsNullOrEmpty(category))
                return workoutsDTOs;

            return workoutsDTOs.Where(w => w.Category.Name == category).ToList();
        }
        public List<WorkoutCardDTO> ApplyDifficultyLevelFilter(List<WorkoutCardDTO> workoutsDTOs, string difficultyLevelGroup)
        {
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
    }
}
