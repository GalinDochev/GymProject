using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.SeedDatabase
{
    internal class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        private readonly SeedData _seedData;

        public ExerciseConfiguration(SeedData seedData)
        {
            _seedData = seedData;
        }

        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasData(_seedData.Exercises);
        }
    }
}
