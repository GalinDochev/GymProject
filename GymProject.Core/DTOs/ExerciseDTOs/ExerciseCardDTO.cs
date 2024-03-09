using GymProject.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.DTOs.ExerciseDTOs
{
    public class ExerciseCardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DifficultyLevel { get; set; }
        public int Repetitions { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Sets { get; set; }
        public List<string>MuscleGroups { get; set; } = new List<string>();

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }


    }
}
