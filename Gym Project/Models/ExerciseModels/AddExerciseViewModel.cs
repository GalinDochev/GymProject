using GymProject.Common.Constants.ExcerciseDataConstants;
using GymProject.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Gym_Project.Models.ExerciseModels
{
    public class AddExerciseViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(ExcerciseDataConstants.MaxExcerciseNameLength, MinimumLength = ExcerciseDataConstants.MinExcerciseNameLength)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(ExcerciseDataConstants.MaxExcerciseDescriptionLength, MinimumLength = ExcerciseDataConstants.MinExcerciseDescriptionLength)]
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(ExcerciseDataConstants.MinExcerciseLevelLength, ExcerciseDataConstants.MaxExcerciseLevelLength)]
        public int DifficultyLevel { get; set; }

        [Required]
        [Range(ExcerciseDataConstants.MinExcerciseRepsLength, ExcerciseDataConstants.MaxExcerciseRepsLength)]
        public int Repetitions { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(ExcerciseDataConstants.MinExcerciseSetsLength, ExcerciseDataConstants.MaxExcerciseSetsLength)]
        public int Sets { get; set; }

        [AtLeastOneCheckedMuscleGroupAttribute(ErrorMessage = "At least one muscle group must be selected.")]
        public List<string> SelectedMuscleGroups { get; set; } = new List<string>();
    }
}
