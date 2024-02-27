using GymProject.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Models
{
    public class ExerciseMuscleGroup : IDeletable
    {
        [ForeignKey(nameof(ExerciseId))]
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;

        [ForeignKey(nameof(MuscleGroupId))]
        public int MuscleGroupId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }= null!;

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }
    }
}
