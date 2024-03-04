using GymProject.Infrastructure.Data.Models;
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

    }
}
