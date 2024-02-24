using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GymProject.Infrastructure.Constants.WorkoutDataConstants;
namespace GymProject.Infrastructure.Data.Models
{
    public class Workout
    {
        [Key]
        [Comment("Workout identifier")]
        public int Id { get; set; }

        [MaxLength(MaxWorkoutNameLength)]
        [Required]
        [Comment("Workout Name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(MaxWorkoutDescriptionLength)]
        [Required]
        [Comment("Workout Description and tips")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Comment("A picture representing the workout")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Comment("Workout Duration in minutes")]
        public int Duration { get; set; }

        [Required]
        [Range(MinWorkoutLevelLength,MaxWorkoutLevelLength)]
        [Comment("Workout Difficulty 1-10 levels")]
        public int DifficultyLevel { get; set; }

        [Required]
        [Comment("Workout Creator either User or Admin")]
        [ForeignKey(nameof(CreatorId))]
        public string CreatorId { get; set; } = string.Empty;
        [Required]
        public IdentityUser Creator { get; set; } = null!;

        [Required]
        [Comment("Workout Category")]
        [ForeignKey(nameof(CategoryId))]
        public int CategoryId { get; set; }
        [Required]
        public Category Category { get; set; } = null!;

        public ICollection<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
    }
}
