using GymProject.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public class ExerciseWorkoutRepository : Repository<ExerciseWorkout>
    {
        private readonly ApplicationDbContext context;
        public ExerciseWorkoutRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
