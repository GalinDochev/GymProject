using GymProject.Common.Constants.ExcerciseDataConstants;
using GymProject.Common.Constants.WorkoutDataConstants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.DTOs.TrainerDTOs
{
    public class ExerciseForTrainerDTO
    {
        [Key]
        [Comment("Excercise identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(ExcerciseDataConstants.MaxExcerciseNameLength, MinimumLength = ExcerciseDataConstants.MinExcerciseNameLength)]
        [Comment("Excercise Name")]
        public string Name { get; set; } = string.Empty;
    }
}
