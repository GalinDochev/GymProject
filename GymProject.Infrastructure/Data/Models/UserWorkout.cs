using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymProject.Infrastructure.Data.Interfaces;

namespace GymProject.Infrastructure.Data.Models
{
    public class UserWorkout : IDeletable
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
        public ApplicationUser User { get; set; } = null!;

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }

    }
}
