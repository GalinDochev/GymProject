using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Models
{
    public class UserWorkout
    {
        [Required]
        [ForeignKey(nameof(WorkoutId))]
        public int WorkoutId { get; set; }

        [Required]
        public Workout Workout { get; set; } = null!;

        [Key]
        [Required]
        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public IdentityUser User { get; set; } = null!;
    }
}
