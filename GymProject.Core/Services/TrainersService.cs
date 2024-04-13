using GymProject.Core.DTOs.TrainerDTOs;
using GymProject.Infrastructure.Data.Interfaces;
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
    public class TrainersService
    {
        private TrainersRepository trainerRepository;
        private ExerciseRepository exerciseRepository;
        private ILogger<TrainersService> _logger;
        public TrainersService(Repository<Trainer> trainersRepo, Repository<Exercise> exerciseRepo,ILogger<TrainersService> logger)
        {
            trainerRepository = (TrainersRepository)trainersRepo;
            exerciseRepository = (ExerciseRepository)exerciseRepo;
            _logger = logger;
        }

        public async Task<List<TrainerCardDTO>> GetAllNotDeletedTrainers()
        {
            try
            {
                var trainers = await trainerRepository.GetAllNotDeleted();
                var trainersDTOs = trainers.Select(t => new TrainerCardDTO
                {
                    FullName = t.FullName,
                    Age = t.Age,
                    Id = t.Id,
                    ImageUrl = t.ImageUrl,
                    Slogan = t.Slogan
                }
                ).ToList();
                return trainersDTOs;
            }

            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred in GetAllNotDeletedTrainers method.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllNotDeletedTrainers method.");
                throw;
            }
        }
       
        public async Task <TrainerProfileDTO> GetTrainerByIdForProfile(int Id)
        {
            try
            {
                var trainer = await trainerRepository.GetById(Id);
                Exercise exercise = await exerciseRepository.GetById(trainer.ExerciseId);
                var trainerDTO = new TrainerProfileDTO
                {
                    FullName = trainer.FullName,
                    Age = trainer.Age,
                    Id = trainer.Id,
                    Slogan = trainer.Slogan,
                    Education = trainer.Education,
                    FavouriteExercise = exercise.Name,
                    ImageUrl = trainer.ImageUrl,
                    IsDeleted = trainer.IsDeleted,
                    DeleteTime = trainer.DeleteTime
                };
                return trainerDTO;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while getting exercise details for Trainer with Id: {Id}");

                throw;
            }
        }

        public async Task <AddTrainerDTO> GetTrainerDTOModel()
        {
            try
            {
                var exercises = await exerciseRepository.GetAllNotDeleted();
                var exerciSelected = exercises.Select(x => new ExerciseForTrainerDTO { Id = x.Id, Name = x.Name }).ToList();
                var model = new AddTrainerDTO { Exercises = exerciSelected };
                return model;
            }

            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SqlException occurred while getting the trainer DTO model.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the trainer DTO model.");
                throw;
            }
        }

        public async Task AddTrainer(AddTrainerDTO trainer)
        {
            try
            {
                var trainerToAdd = new Trainer
                {
                    Id = trainer.Id,
                    Age = trainer.Age,
                    Education = trainer.Education,
                    ExerciseId = trainer.ExerciseId,
                    FullName = trainer.FullName,
                    ImageUrl = trainer.ImageUrl,
                    Slogan = trainer.Slogan
                };
                await trainerRepository.Add(trainerToAdd);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred during the database update when adding a trainer.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation occurred when adding a trainer.");
                throw;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An SQL exception occurred when adding a trainer.");
                throw;
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "A timeout exception occurred when adding a trainer.");
                throw;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "A not supported exception occurred when adding a trainer.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when adding a trainer.");
                throw;
            }
        }

        public async Task<AddTrainerDTO> GetTrainerByIdForEdit (int id)
        {
            try
            {
                var trainer = await trainerRepository.GetById(id);
                var exercises = await exerciseRepository.GetAllNotDeleted();
                var exerciSelected = exercises.Select(x => new ExerciseForTrainerDTO { Id = x.Id, Name = x.Name }).ToList();
                var trainerDTO = new AddTrainerDTO
                {
                    Id = trainer.Id,
                    Age = trainer.Age,
                    Education = trainer.Education,
                    ExerciseId = trainer.ExerciseId,
                    FullName = trainer.FullName,
                    ImageUrl = trainer.ImageUrl,
                    Slogan = trainer.Slogan,
                    Exercises = exerciSelected
                };
                return trainerDTO;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation occurred when getting an trainer for edit.");
                throw;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An SQL exception occurred when getting an trainer for edit.");
                throw;
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "A timeout exception occurred when getting an trainer for edit.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when getting an trainer for edit.");
                throw;
            }
        }

        public async Task EditTrainer(AddTrainerDTO trainerDTO)
        {
            try
            {
                var trainerToEdit = await trainerRepository.GetById(trainerDTO.Id);
                trainerToEdit.Id = trainerDTO.Id;
                trainerToEdit.FullName = trainerDTO.FullName;
                trainerToEdit.Age = trainerDTO.Age;
                trainerToEdit.ExerciseId = trainerDTO.ExerciseId;
                trainerToEdit.Education = trainerDTO.Education;
                trainerToEdit.Slogan = trainerDTO.Slogan;
                trainerToEdit.ImageUrl = trainerDTO.ImageUrl;
                await trainerRepository.Update(trainerToEdit);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while editing trainer.");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while editing trainer.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation occurred while editing trainer.");
                throw;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Operation not supported error occurred while editing trainer.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while editing trainer.");
                throw;
            }
        }

        public async Task DeleteTrainer(int Id)
        {
            try
            {
                var trainerForDelete = await trainerRepository.GetById(Id);

                await trainerRepository.Delete(trainerForDelete);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while deleting trainer.");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while deleting trainer.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation occurred while deleting trainer.");
                throw;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Operation not supported error occurred while deleting trainer.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting trainer.");
                throw;
            }
        }
    }
}
