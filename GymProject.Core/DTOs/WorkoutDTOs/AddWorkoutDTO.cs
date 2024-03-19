using GymProject.Common.Constants.WorkoutDataConstants;
using GymProject.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymProject.Common.Constants.ExcerciseDataConstants;

namespace GymProject.Core.DTOs.WorkoutDTOs
{
    public class AddWorkoutDTO
    {
        [Key]
        [Comment("Workout identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(WorkoutDataConstants.MaxWorkoutNameLength, MinimumLength = WorkoutDataConstants.MinWorkoutNameLength)]
        [Comment("Workout Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(WorkoutDataConstants.MaxWorkoutDescriptionLength, MinimumLength = WorkoutDataConstants.MinWorkoutDescriptionLength)]
        [Comment("Workout Description and tips")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Comment("A picture representing the workout")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be greater than {1}.")]
        [Comment("Workout Duration in minutes")]
        public int Duration { get; set; }

        [Required]
        [Range(WorkoutDataConstants.MinWorkoutLevelLength, WorkoutDataConstants.MaxWorkoutLevelLength, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Comment("Workout Difficulty 1-10 levels")]
        public int DifficultyLevel { get; set; }

        [Required]
        [Comment("Workout Creator either User or Admin")]
        [ForeignKey(nameof(CreatorId))]
        public string CreatorId { get; set; } = string.Empty;

        [Required]
        [Comment("Workout Category")]
        [ForeignKey(nameof(CategoryId))]
        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; } = null!;

        public ICollection<Exercise> SelectedExercises { get; set; } = new List<Exercise>();
        public ICollection<Category> SelectedCategories{ get; set; } = new List<Category>();
    }
}
