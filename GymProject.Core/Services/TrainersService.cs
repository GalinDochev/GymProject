using GymProject.Core.DTOs.TrainerDTOs;
using GymProject.Infrastructure.Data.Interfaces;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
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
        public TrainersService(Repository<Trainer> trainersRepo, Repository<Exercise> exerciseRepo)
        {
            trainerRepository = (TrainersRepository)trainersRepo;
            exerciseRepository = (ExerciseRepository)exerciseRepo;
        }

        public async Task<List<TrainerCardDTO>> GetAllNotDeletedTrainers()
        {
           var trainers= await trainerRepository.GetAllNotDeleted();
            var trainersDTOs = trainers.Select(t => new TrainerCardDTO
            {
                FullName = t.FullName,
                Age = t.Age,
                Id = t.Id,
                ImageUrl= t.ImageUrl,
                Slogan=t.Slogan
            }
            ).ToList();
           return trainersDTOs;
        }
       
        public async Task <TrainerProfileDTO> GetTrainerByIdForProfile(int Id)
        {
            var trainer = await trainerRepository.GetById(Id);
            Exercise exercise=await exerciseRepository.GetById(trainer.ExerciseId);
            var trainerDTO = new TrainerProfileDTO
            {
                FullName = trainer.FullName,
                Age=trainer.Age,
                Id = trainer.Id,
                Slogan=trainer.Slogan,
                Education=trainer.Education,
                FavouriteExercise= exercise.Name,
                ImageUrl=trainer.ImageUrl,
                IsDeleted=trainer.IsDeleted,
                DeleteTime=trainer.DeleteTime
            };
            return trainerDTO;
        }

        public async Task <AddTrainerDTO> GetTrainerView()
        {
            var exercises=await exerciseRepository.GetAllNotDeleted();
            var exerciSelected=exercises.Select(x => new ExerciseForTrainerDTO { Id=x.Id, Name=x.Name}).ToList();
            var model = new AddTrainerDTO { Exercises = exerciSelected };
            return model;
        }

        public async Task AddTrainer(AddTrainerDTO trainer)
        {
            var trainerToAdd = new Trainer {
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

        public async Task<AddTrainerDTO> GetTrainerByIdForEdit (int id)
        {
            var trainer= await trainerRepository.GetById(id);
            var  exercises = await exerciseRepository.GetAllNotDeleted();
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
                Exercises=exerciSelected
            };
            return trainerDTO;
        }

        public async Task EditTrainer(AddTrainerDTO trainerDTO)
        {
            var trainerToEdit = await trainerRepository.GetById(trainerDTO.Id);
            trainerToEdit.Id = trainerDTO.Id;
            trainerToEdit.FullName = trainerDTO.FullName;
            trainerToEdit.Age=trainerDTO.Age;
            trainerToEdit.ExerciseId = trainerDTO.ExerciseId;
            trainerToEdit.Education= trainerDTO.Education;
            trainerToEdit.Slogan = trainerDTO.Slogan;
            trainerToEdit.ImageUrl= trainerDTO.ImageUrl;
            await trainerRepository.Update(trainerToEdit);
        }

        public async Task DeleteTrainer(int Id)
        {
            var trainerForDelete=await trainerRepository.GetById(Id);

            await trainerRepository.Delete(trainerForDelete);
        }
    }
}
