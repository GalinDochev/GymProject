using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.DTOs.ExerciseDTOs
{
    public class ExerciseDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DifficultyLevel { get; set; }
        public int Repetitions { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Sets { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<string> MuscleGroups { get; set; } = new List<string>();
    }
}
