using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.SeedDatabase
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        private readonly SeedData _seedData;

        public CategoryConfiguration(SeedData seedData)
        {
            _seedData = seedData;
        }

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(_seedData.Categories);
        }
    }
}
