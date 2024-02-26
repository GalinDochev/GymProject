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
        public void Configure(EntityTypeBuilder<MuscleGroup> builder)
        {
            var data = new SeedData();
            builder.HasData(new MuscleGroup[]
            {
                data.FirstMuscleGroup,
                data.SecondMuscleGroup,
                data.ThirdMuscleGroup,
                data.FourthMuscleGroup,
                data.FifthMuscleGroup,
                data.SixthMuscleGroup,
                data.SeventhMuscleGroup,
                data.EightMuscleGroup
            }
            );
        }
    }
}
