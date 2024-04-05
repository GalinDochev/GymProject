using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        public WorkoutRepository(ApplicationDbContext context,ILogger<WorkoutRepository> logger) : base(context)
        {
            this.context = context;
            _logger = logger;
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
            try
            {
                var workout = await context.Workouts
                    .Where(w => w.IsDeleted == false)
                    .Include(w => w.Category)
                    .Include(uw => uw.UsersWorkouts)
                        .ThenInclude(uw => uw.User)
                    .Include(w => w.ExerciseWorkouts)
                        .ThenInclude(ew => ew.Exercise)
                    .Where(w => !w.UsersWorkouts.All(emg => emg.IsDeleted == true)
                            && !w.ExerciseWorkouts.All(emg => emg.IsDeleted == true))
                    .FirstOrDefaultAsync(w => w.Id == id);

                if (workout == null)
                {
                    throw new InvalidOperationException($"Workout with id {id} is not found.");
                }

                return workout;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Workout with id {id} is not found in the Database.");

                throw;
            }
        }
    }
}
