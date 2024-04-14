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
    internal class MuscleGroupConfiguration : IEntityTypeConfiguration<MuscleGroup>
    {
        private readonly SeedData _seedData;

        public MuscleGroupConfiguration(SeedData seedData)
        {
            _seedData = seedData;
        }

        public void Configure(EntityTypeBuilder<MuscleGroup> builder)
        {
            builder.HasData(_seedData.MuscleGroups);
        }
    }
}
