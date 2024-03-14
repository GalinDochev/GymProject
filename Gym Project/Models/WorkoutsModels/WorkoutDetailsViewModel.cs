using GymProject.Infrastructure.Data.Models;

namespace Gym_Project.Models.WorkoutsModels
{
    public class WorkoutDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int DifficultyLevel { get; set; }

        public string CreatorId { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
    }
}
