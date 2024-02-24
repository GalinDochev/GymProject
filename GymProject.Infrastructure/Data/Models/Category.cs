using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static GymProject.Infrastructure.Constants.WorkoutDataConstants;
namespace GymProject.Infrastructure.Data.Models
{
    public class Category
    {
        [Key]
        [Comment("Category Identifier")]
        public int Id { get; set; }

        [MaxLength(MaxWorkoutCategoryNameLength)]
        [Required]
        [Comment("Category Name")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}
