using GymProject.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Gym_Project.Models.WorkoutsModels
{
    public class AllWorkoutsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int DifficultyLevel { get; set; }
        public string CreatorId { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;
        public string Category { get; set; } = null!;

        public bool IsJoined { get; set; }
    }
}
