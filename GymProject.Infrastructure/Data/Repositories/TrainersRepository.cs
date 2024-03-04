using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public class TrainersRepository : Repository<Trainer>
    {
        private readonly ApplicationDbContext context;

        public TrainersRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

    }
}
