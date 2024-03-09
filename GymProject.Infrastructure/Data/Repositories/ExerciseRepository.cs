using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public class ExerciseRepository : Repository<Exercise>
    {
        private readonly ApplicationDbContext context;

        public ExerciseRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<Exercise>> GetAllNotDeleted()
        {
            var exercises = await context.Excercises
                .Where(e => e.IsDeleted == false)
                .Include(e => e.ExerciseMuscleGroups)
                    .ThenInclude(emg => emg.MuscleGroup)
                .Where(e => !e.ExerciseMuscleGroups.Any(emg => emg.IsDeleted == true))
                .ToListAsync();

            return exercises;
        }


    }
}
