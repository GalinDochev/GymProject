using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Models
{
    public class ExerciseWorkout
    {
        [ForeignKey(nameof(ExerciseId))]
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;

        [ForeignKey(nameof(WorkoutId))]
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;
    }
}
