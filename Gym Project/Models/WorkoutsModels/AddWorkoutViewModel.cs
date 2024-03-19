using GymProject.Common.Constants.WorkoutDataConstants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Gym_Project.Models.WorkoutsModels
{
    public class AddWorkoutViewModel
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

        [Comment("Workout Creator either User or Admin")]
        [ForeignKey(nameof(CreatorId))]
        public string CreatorId { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = null!;

        [AtLeastOneCheckedAttribute(ErrorMessage = "At least one exercise must be selected.")]
        public List<string> SelectedExercises{ get; set; } = new List<string>();
        public List<string> SelectedCategories{ get; set; } = new List<string>();
    }
}
