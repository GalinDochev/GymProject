using GymProject.Common.Constants.ExcerciseDataConstants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.DTOs
{
    public class ExerciseForTrainerDTO
    {
        [Key]
        [Comment("Excercise identifier")]
        public int Id { get; set; }

        [MaxLength(ExcerciseDataConstants.MaxExcerciseNameLength)]
        [Required]
        [Comment("Excercise Name")]
        public string Name { get; set; } = string.Empty;
    }
}
