using GymProject.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public class ExerciseMuscleGroupRepository : Repository<ExerciseMuscleGroup>
    {
        private readonly ApplicationDbContext context;

        public ExerciseMuscleGroupRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task RevertDelete(int id)
        {

        }
    }
}
