using GymProject.Core.DTOs;
using GymProject.Infrastructure.Data.Interfaces;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Services
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
            var exercises=await exerciseRepository.GetAll();
            var exerciSelected=exercises.Select(x => new ExerciseForTrainerDTO { Id=x.Id, Name=x.Name}).ToList();

            var model = new AddTrainerDTO { Exercises = exerciSelected ,Age=18};
            return model;
        }

    }
}
