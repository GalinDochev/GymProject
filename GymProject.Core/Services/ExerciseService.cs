using GymProject.Core.DTOs;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.Services
{
    public class ExerciseService
    {
        private ExerciseRepository exerciseRepository;
        public ExerciseService(Repository<Exercise> exerciseRepo)
        {
                exerciseRepository=(ExerciseRepository)exerciseRepo;
        }

        public async Task<List<ExerciseForTrainerDTO>> GetAllNotDeletedExForTrainers()
        {
            var exercises = await exerciseRepository.GetAllNotDeleted();
            var exercisesDTOs = exercises.Select(e=>new ExerciseForTrainerDTO
            {
                 Id=e.Id,
                 Name=e.Name
            }).ToList();
            return exercisesDTOs;
        }
    }
}
