using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public class MuscleGroupRepository : Repository<MuscleGroup>
    {
        private readonly ApplicationDbContext context;

        public MuscleGroupRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<MuscleGroup>> GetMuscleGroupsByName(List<string> muscleGroupNames)
        {
            return await context.MuscleGroups
                .Where(m => muscleGroupNames.Contains(m.Name))
                .ToListAsync();
        }
    }
}
