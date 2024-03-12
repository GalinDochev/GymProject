using GymProject.Common.Constants.ExcerciseDataConstants;
using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.DTOs.ExerciseDTOs
{
    public class AddExerciseDTO
    {

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int DifficultyLevel { get; set; }
        public int Repetitions { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int Sets { get; set; }

        public ICollection<MuscleGroup> SelectedMuslceGroups { get; set; } = new List<MuscleGroup>();
        public ICollection<string> MuscleGroupNames { get; set; } = new List<string>();
    }
}
