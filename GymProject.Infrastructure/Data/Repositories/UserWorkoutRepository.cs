using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public class UserWorkoutRepository: Repository<UserWorkout>
    {
        private readonly ApplicationDbContext context;
        public UserWorkoutRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task <UserWorkout> GetByUserIdAndWorkoutId(string userId,int workoutId)
        {
            var userWorkout = await context.UsersWorkouts
               .Where(uw => uw.UserId == userId && uw.WorkoutId == workoutId).FirstOrDefaultAsync();

            return userWorkout;
        }

        public async Task<bool> AlreadyJoinedCheck(string userId, int workoutId)
        {
            var userWorkout = await context.UsersWorkouts
               .AnyAsync(uw => uw.UserId == userId && uw.WorkoutId == workoutId);

            return userWorkout;
        }

    }
}
