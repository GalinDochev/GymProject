using GymProject.Core.DTOs.ExerciseDTOs;
using GymProject.Core.DTOs.TrainerDTOs;
using GymProject.Core.Exceptions;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymProject.Core.Services
{
    public class ExerciseService
    {
        private ExerciseRepository exerciseRepository;
        private ExerciseMuscleGroupRepository exerciseMuscleGroupRepository;
        private MuscleGroupRepository muscleGroupRepository;
        private ExerciseWorkoutRepository exerciseWorkoutRepository;
        private readonly ILogger<ExerciseService> _logger;
        public ExerciseService
            (
            Repository<Exercise> exerciseRepo,
            Repository<ExerciseMuscleGroup> exerciseMuscleGroupRepo,
            Repository<MuscleGroup>muscleGroupRepo,
            Repository<ExerciseWorkout>exerciseWorkoutRepo,
            ILogger<ExerciseService> logger
            )
        {
            exerciseRepository = (ExerciseRepository)exerciseRepo;
            exerciseMuscleGroupRepository = (ExerciseMuscleGroupRepository)exerciseMuscleGroupRepo;
            muscleGroupRepository = (MuscleGroupRepository)muscleGroupRepo;
            exerciseWorkoutRepository = (ExerciseWorkoutRepository)exerciseWorkoutRepo;
            _logger = logger;
        }

        public async Task<List<ExerciseForTrainerDTO>> GetAllNotDeletedExForTrainers()
        {
            try
            {
                var exercises = await exerciseRepository.GetAllNotDeleted();

                var exercisesDTOs = exercises.Select(e => new ExerciseForTrainerDTO
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToList();
                return exercisesDTOs;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An InvalidOperationException occurred in GetAllNotDeletedExForTrainers method.");
                throw;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred in GetAllNotDeletedExForTrainers method.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllNotDeletedExForTrainers method.");
                throw;
            }
        }

        public async Task<List<ExerciseCardDTO>> GetAllNotDeletedExercises()
        {
            try
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
                    MuscleGroups = e.ExerciseMuscleGroups.Where(emg => emg.IsDeleted == false).Select(emg => emg.MuscleGroup.Name).ToList()
                }).ToList();
                return exercisesDTOs;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred in GetAllNotDeletedExercises method.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllNotDeletedExercises method.");
                throw;
            }
        }

        public async Task<ExerciseDetailsDTO> GetExerciseByIdForDetails(int Id)
        {
            try
            {
                var exercise = await exerciseRepository.GetById(Id);
                var exerciseDTO = new ExerciseDetailsDTO
                {
                    Name = exercise.Name,
                    Id = exercise.Id,
                    Description = exercise.Description,
                    DifficultyLevel = exercise.DifficultyLevel,
                    ImageUrl = exercise.ImageUrl,
                    MuscleGroups = exercise.ExerciseMuscleGroups.Select(emg => emg.MuscleGroup.Name).ToList(),
                    Repetitions = exercise.Repetitions,
                    Sets = exercise.Sets,
                };
                return exerciseDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting exercise details for Exercise with Id: {Id}");

                throw;
            }
        }

        public async Task<AddExerciseDTO> GetExerciseDTOModel()
        {
            try
            {
                var muscleGroups = await muscleGroupRepository.GetAllNotDeleted();

                var model = new AddExerciseDTO
                {
                    SelectedMuslceGroups = muscleGroups.ToList()
                };

                return model;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred while getting the exercise DTO model.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the exercise DTO model.");
                throw;
            }
        }

        public async Task<List<string>> GetMuscleGroupsNames()
        {
            try
            {
                var muscleGroups = await muscleGroupRepository.GetMuscleGroupsNames();
                return muscleGroups;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred while getting muscle group names.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting muscle group names.");
                throw;
            }
        }

        public async Task<List<string>> GetExercisesNames()
        {
            try
            {
                var exercisesNames = await exerciseRepository.GetExercisesNames();
                return exercisesNames;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred in GetExercisesNames method.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetExercisesNames method.");
                throw;
            }
        }

        public async Task<List<MuscleGroup>> GetMuscleGroupsByName(List<string> muscleGroupNames)
        {
            try
            {
                var muscleGroups = await muscleGroupRepository.GetMuscleGroupsByName(muscleGroupNames);
                return muscleGroups;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred in GetMuscleGroupsByName method.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetMuscleGroupsByName method.");
                throw;
            }
        }

        public async Task<List<Exercise>> GetExercisesByName(List<string> exercisesNames)
        {

            try
            {
                var exercises = await exerciseRepository.GetExercisesByName(exercisesNames);
                return exercises;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred in GetMuscleGroupsByName method.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetMuscleGroupsByName method.");
                throw;
            }
        }

        public async Task AddExercise(AddExerciseDTO exercise)
        {
            try
            {
                var exerciseToAdd = new Exercise
                {
                    Id = exercise.Id,
                    Description = exercise.Description,
                    DifficultyLevel = exercise.DifficultyLevel,
                    Name = exercise.Name,
                    Sets = exercise.Sets,
                    Repetitions = exercise.Repetitions,
                    ImageUrl = exercise.ImageUrl,
                };
                foreach (var muscleGroup in exercise.SelectedMuslceGroups)
                {
                    var exerciseMuscleGroup = new ExerciseMuscleGroup
                    {
                        ExerciseId = exerciseToAdd.Id,
                        MuscleGroupId = muscleGroup.Id
                    };

                    exerciseToAdd.ExerciseMuscleGroups.Add(exerciseMuscleGroup);
                }
                await exerciseRepository.Add(exerciseToAdd);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred during the database update when adding an exercise.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation occurred when adding an exercise.");
                throw;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An SQL exception occurred when adding an exercise.");
                throw;
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "A timeout exception occurred when adding an exercise.");
                throw;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "A not supported exception occurred when adding an exercise.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when adding an exercise.");
                throw;
            }
        }

        public async Task<AddExerciseDTO> GetExerciseByIdForEdit(int id)
        {
            try
            {
                var exercise = await exerciseRepository.GetById(id);
                var selectedMuscleGroups = await muscleGroupRepository.GetAllNotDeleted();
                var exerciseDTO = new AddExerciseDTO
                {
                    Id = exercise.Id,
                    Description = exercise.Description,
                    DifficultyLevel = exercise.DifficultyLevel,
                    ImageUrl = exercise.ImageUrl,
                    Name = exercise.Name,
                    Repetitions = exercise.Repetitions,
                    Sets = exercise.Sets,
                    SelectedMuslceGroups = selectedMuscleGroups.ToList()
                };
                return exerciseDTO;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An SQL exception occurred when getting an exercise for edit.");
                throw;
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "A timeout exception occurred when getting an exercise for edit.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when getting an exercise for edit.");
                throw;
            }
        }

        public async Task EditExercise(AddExerciseDTO exerciseDTO)
        {
            try
            {
                var exerciseToEdit = await exerciseRepository.GetById(exerciseDTO.Id);
                exerciseToEdit.Id = exerciseDTO.Id;
                exerciseToEdit.Name = exerciseDTO.Name;
                exerciseToEdit.DifficultyLevel = exerciseDTO.DifficultyLevel;
                exerciseToEdit.Description = exerciseDTO.Description;
                exerciseToEdit.Repetitions = exerciseDTO.Repetitions;
                exerciseToEdit.Sets = exerciseDTO.Sets;
                exerciseToEdit.ImageUrl = exerciseDTO.ImageUrl;

                var existingMuscleGroups = exerciseToEdit.ExerciseMuscleGroups.ToList();

                foreach (var existingMuscleGroup in existingMuscleGroups)
                {
                    if (!exerciseDTO.SelectedMuslceGroups.Any(mg => mg.Id == existingMuscleGroup.MuscleGroupId))
                    {

                        if (!existingMuscleGroup.IsDeleted)
                        {
                            await exerciseMuscleGroupRepository.Delete(existingMuscleGroup);
                        }
                    }
                }

                foreach (var muscleGroup in exerciseDTO.SelectedMuslceGroups)
                {
                    var existingAssociation = existingMuscleGroups.FirstOrDefault(emg => emg.MuscleGroupId == muscleGroup.Id);

                    if (existingAssociation == null)
                    {
                        var exerciseMuscleGroup = new ExerciseMuscleGroup
                        {
                            ExerciseId = exerciseToEdit.Id,
                            MuscleGroupId = muscleGroup.Id
                        };

                        exerciseToEdit.ExerciseMuscleGroups.Add(exerciseMuscleGroup);

                        await exerciseMuscleGroupRepository.Add(exerciseMuscleGroup);
                    }
                    else if (existingAssociation.IsDeleted)
                    {

                        existingAssociation.IsDeleted = false;
                        existingAssociation.DeleteTime = default;
                        await exerciseMuscleGroupRepository.Update(existingAssociation);
                    }
                }
                await exerciseRepository.Update(exerciseToEdit);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while editing exercise.");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while editing exercise.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation occurred while editing exercise.");
                throw;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Operation not supported error occurred while editing exercise.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while editing exercise.");
                throw;
            }
        }

        public async Task DeleteExercise(int Id)
        {
            try
            {
                var exerciseToDelete = await exerciseRepository.GetById(Id);
                foreach (var exerciseMuscleGroup in exerciseToDelete.ExerciseMuscleGroups)
                {
                    await exerciseMuscleGroupRepository.Delete(exerciseMuscleGroup);
                }
                foreach (var exerciseWorkout in exerciseToDelete.ExerciseWorkouts)
                {
                    await exerciseWorkoutRepository.Delete(exerciseWorkout);
                }

                await exerciseRepository.Delete(exerciseToDelete);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while deleting exercise.");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while deleting exercise.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation occurred while deleting exercise.");
                throw;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Operation not supported error occurred while deleting exercise.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting exercise.");
                throw;
            }

        }

    }
}
