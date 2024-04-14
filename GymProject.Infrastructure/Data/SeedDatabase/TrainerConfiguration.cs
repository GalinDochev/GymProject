using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.SeedDatabase
{
    internal class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        private readonly SeedData _seedData;

        public TrainerConfiguration(SeedData seedData)
        {
            _seedData = seedData;
        }
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Trainer> builder)
        {
            builder.HasData(_seedData.Trainers);
        }
    }
}
