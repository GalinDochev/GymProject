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
        private ExerciseMuscleGroupRepository exerciseMuscleGroupRepository;
        private MuscleGroupRepository muscleGroupRepository;
        public ExerciseService
            (
            Repository<Exercise> exerciseRepo,
            Repository<ExerciseMuscleGroup> exerciseMuscleGroupRepo,
            Repository<MuscleGroup>muscleGroupRepo
            )
        {
            exerciseRepository = (ExerciseRepository)exerciseRepo;
            exerciseMuscleGroupRepository = (ExerciseMuscleGroupRepository)exerciseMuscleGroupRepo;
            muscleGroupRepository = (MuscleGroupRepository)muscleGroupRepo;
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
                MuscleGroups = e.ExerciseMuscleGroups.Where(emg=>emg.IsDeleted==false).Select(emg => emg.MuscleGroup.Name).ToList()
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
                Description = exercise.Description,
                DifficultyLevel = exercise.DifficultyLevel,
                ImageUrl = exercise.ImageUrl,
                MuscleGroups = exercise.ExerciseMuscleGroups.Select(emg => emg.MuscleGroup.Name).ToList(),
                Repetitions = exercise.Repetitions,
                Sets = exercise.Sets,
            };
            return exerciseDTO;
        }

        public async Task<AddExerciseDTO> GetExerciseViewModel()
        {
            var muscleGroups = await muscleGroupRepository.GetAllNotDeleted();
            
            var model = new AddExerciseDTO
            {
                SelectedMuslceGroups = muscleGroups.ToList()
            };

            return model;
        }

        public async Task<List<string>> GetMuscleGroupsNames()
        {
            var muscleGroups = await muscleGroupRepository.GetAllNotDeleted();
            var muscleGroupsNames = muscleGroups.Select(m => m.Name).ToList();
            return muscleGroupsNames;
        }

        public async Task<List<MuscleGroup>> GetMuscleGroupsByName(List<string> muscleGroupNames)
        {

            var muscleGroups = await muscleGroupRepository.GetMuscleGroupsByName(muscleGroupNames);
            return muscleGroups;
        }

        public async Task AddExercise(AddExerciseDTO exercise)
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

        public async Task<AddExerciseDTO> GetExerciseByIdForEdit(int id)
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

        public async Task EditExercise(AddExerciseDTO exerciseDTO)
        {
            var exerciseToEdit = await exerciseRepository.GetById(exerciseDTO.Id);
            exerciseToEdit.Id = exerciseDTO.Id;
            exerciseToEdit.Name = exerciseDTO.Name;
            exerciseToEdit.DifficultyLevel = exerciseDTO.DifficultyLevel;
            exerciseToEdit.Description = exerciseDTO.Description;
            exerciseToEdit.Repetitions = exerciseDTO.Repetitions;
            exerciseToEdit.Sets = exerciseDTO.Sets;
            exerciseToEdit.ImageUrl = exerciseDTO.ImageUrl;

            // Extract the IDs of existing muscle groups associated with the exercise
            var existingMuscleGroups = exerciseToEdit.ExerciseMuscleGroups.ToList();

            // Iterate over the existing ExerciseMuscleGroups to identify and delete unused ones
            foreach (var existingMuscleGroup in existingMuscleGroups)
            {
                // Check if the existing association is with a selected muscle group
                if (!exerciseDTO.SelectedMuslceGroups.Any(mg => mg.Id == existingMuscleGroup.MuscleGroupId))
                {
                    // If the existing association is not with a selected muscle group,
                    // soft delete it if it's not already deleted
                    if (!existingMuscleGroup.IsDeleted)
                    {
                       await exerciseMuscleGroupRepository.Delete(existingMuscleGroup);
                    }
                }
            }

            foreach (var muscleGroup in exerciseDTO.SelectedMuslceGroups)
            {
                // Check if an association already exists (including soft deleted associations)
                var existingAssociation = existingMuscleGroups.FirstOrDefault(emg => emg.MuscleGroupId == muscleGroup.Id);

                if (existingAssociation == null)
                {
                    // If no existing association found, create a new one
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
                    existingAssociation.DeleteTime = default; // Reset delete time
                    await exerciseMuscleGroupRepository.Update(existingAssociation);
                }
            }
            await exerciseRepository.Update(exerciseToEdit);
        }

        public async Task DeleteExercise(int Id)
        {
            var exerciseToDelete = await exerciseRepository.GetById(Id);
            foreach (var exerciseMuscleGroup in exerciseToDelete.ExerciseMuscleGroups)
            {
                await exerciseMuscleGroupRepository.Delete(exerciseMuscleGroup);
            }

            await exerciseRepository.Delete(exerciseToDelete);
        }
    }
}
