using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public class WorkoutRepository:Repository<Workout>
    {
        private readonly ApplicationDbContext context;
        public WorkoutRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<Workout>> GetAllNotDeleted()
        {
            var workouts = await context.Workouts
                .Where(w => w.IsDeleted == false)
                .Include(w => w.Category)
                .Include(uw => uw.UsersWorkouts)
                    .ThenInclude(uw => uw.User)
                 .Include(w=>w.ExerciseWorkouts)
                    .ThenInclude(ew=>ew.Exercise)
                .Where
                (w => !w.UsersWorkouts.All(emg => emg.IsDeleted == true) 
                && !w.ExerciseWorkouts.All(emg=>emg.IsDeleted==true))
                .ToListAsync();

            return workouts;
        }
        public override async Task<Workout> GetById(int id)
        {
            var workout = await context.Workouts
                .Where(w => w.IsDeleted == false)
                .Include(w => w.Category)
                .Include(uw => uw.UsersWorkouts)
                    .ThenInclude(uw => uw.User)
                 .Include(w => w.ExerciseWorkouts)
                    .ThenInclude(ew => ew.Exercise)
                .Where
                (w => !w.UsersWorkouts.All(emg => emg.IsDeleted == true)
                && !w.ExerciseWorkouts.All(emg => emg.IsDeleted == true))
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workout == null)
            {
                throw new Exception($"Workout with id {id} is not found.");
            }

            return workout;
        }
    }
}
