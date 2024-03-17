using GymProject.Common.Constants.WorkoutDataConstants;
using GymProject.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.DTOs.WorkoutDTOs
{
    public class WorkoutCardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int DifficultyLevel { get; set; }
        public string CreatorId { get; set; } = string.Empty;
        public IdentityUser Creator { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<UserWorkout> UserWorkouts { get; set; } = new List<UserWorkout>();
    }
}
