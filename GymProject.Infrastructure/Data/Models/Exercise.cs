using GymProject.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static GymProject.Infrastructure.Constants.ExcerciseDataConstants;
namespace GymProject.Infrastructure.Data.Models
{
    public class Exercise : IDeletable
    {
        [Key]
        [Comment("Excercise identifier")]
        public int Id { get; set; }

        [MaxLength(MaxExcerciseNameLength)]
        [Required]
        [Comment("Excercise Name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(MaxExcerciseDescriptionLength)]
        [Required]
        [Comment("Excercise Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Comment("Workout Difficulty 1-10 levels")]
        [Range(MinExcerciseLevelLength, MaxExcerciseLevelLength)]
        public int DifficultyLevel { get; set; }
        [Required]
        [Range(MinExcerciseRepsLength, MaxExcerciseRepsLength)]
        public int Repetitions { get; set; }

        [Required]
        [Comment("A picture of the Excercise during motion")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(MinExcerciseSetsLength, MaxExcerciseSetsLength)]
        public int Sets { get; set; }
        public ICollection<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
        public ICollection<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }
    }
}
