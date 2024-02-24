using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GymProject.Infrastructure.Data.Models
{
    using static GymProject.Infrastructure.Constants.ExcerciseDataConstants;
    public class MuscleGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Comment("Name of the muscle group")]
        [MaxLength(MaxMuscleGroupNameLength)]
        public string Name { get; set; } = string.Empty;
        public ICollection<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();
    }
}
