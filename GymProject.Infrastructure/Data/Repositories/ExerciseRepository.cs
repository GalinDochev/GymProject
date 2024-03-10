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

        public override async Task<Exercise> GetById(int id)
        {
            var exercise = await context.Excercises
                .Include(e => e.ExerciseMuscleGroups)
                    .ThenInclude(emg => emg.MuscleGroup)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                throw new Exception($"Exercise with id {id} is not found.");
            }
            
            return exercise;
        }

       public async Task<List<MuscleGroup>> GetAllMuscleGroups()
        {
            var muscleGroups = await context.MuscleGroups.Where(m => m.IsDeleted == false).ToListAsync();
            return muscleGroups;
        }

        public async Task<List<MuscleGroup>> GetMuscleGroupsByName(List<string> muscleGroupNames)
        {
            // Query the database to find MuscleGroup entities with names matching the provided list of names
            return await context.MuscleGroups
                .Where(m => muscleGroupNames.Contains(m.Name))
                .ToListAsync();
        }
    }
}
