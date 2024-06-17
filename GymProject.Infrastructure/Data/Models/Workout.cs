using GymProject.Infrastructure.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymProject.Common.Constants.WorkoutDataConstants;
namespace GymProject.Infrastructure.Data.Models
{
    public class Workout : IDeletable
    {
        [Key]
        [Comment("Workout identifier")]
        public int Id { get; set; }

        [MaxLength(WorkoutDataConstants.MaxWorkoutNameLength)]
        [Required]
        [Comment("Workout Name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(WorkoutDataConstants.MaxWorkoutDescriptionLength)]
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
        [Range(WorkoutDataConstants.MinWorkoutLevelLength, WorkoutDataConstants.MaxWorkoutLevelLength)]
        [Comment("Workout Difficulty 1-10 levels")]
        public int DifficultyLevel { get; set; }

        [Required]
        [Comment("Workout Creator either User or Admin")]
        [ForeignKey(nameof(CreatorId))]
        public string CreatorId { get; set; } = string.Empty;
        [Required]
        public ApplicationUser Creator { get; set; } = null!;

        [Required]
        [Comment("Workout Category")]
        [ForeignKey(nameof(CategoryId))]
        public int CategoryId { get; set; }
        [Required]
        public Category Category { get; set; } = null!;

        [Required]
        public DateTime WorkoutStartTime { get; set; }

        public ICollection<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
        public ICollection<UserWorkout> UsersWorkouts { get; set; } = new List<UserWorkout>();

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }
    }
}
