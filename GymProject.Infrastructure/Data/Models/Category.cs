using GymProject.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using GymProject.Common.Constants.WorkoutDataConstants;
namespace GymProject.Infrastructure.Data.Models
{
    public class Category:IDeletable
    {
        [Key]
        [Comment("Category Identifier")]
        public int Id { get; set; }

        [MaxLength(WorkoutDataConstants.MaxWorkoutCategoryNameLength)]
        [Required]
        [Comment("Category Name")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }
    }
}
