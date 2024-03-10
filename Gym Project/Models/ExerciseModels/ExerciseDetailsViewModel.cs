using GymProject.Common.Constants.ExcerciseDataConstants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gym_Project.Models.ExerciseModels
{
    public class ExerciseDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DifficultyLevel { get; set; }
        public int Repetitions { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Sets { get; set; }
        public string MuscleGroups { get; set; } = string.Empty;
    }
}
