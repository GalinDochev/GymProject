using GymProject.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using GymProject.Common.Constants.ExcerciseDataConstants;

namespace GymProject.Infrastructure.Data.Models
{
    public class MuscleGroup : IDeletable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Comment("Name of the muscle group")]
        [MaxLength(ExcerciseDataConstants.MaxMuscleGroupNameLength)]
        public string Name { get; set; } = string.Empty;
        public ICollection<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }
    }
}
