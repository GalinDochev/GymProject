using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        private readonly ApplicationDbContext context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public virtual async Task <Category> GetCategoryByName(string categoryName)
        {
            return await context.WorkoutCategories
                .Where(m => categoryName.Contains(m.Name))
                .FirstOrDefaultAsync();
        }
    }
}
