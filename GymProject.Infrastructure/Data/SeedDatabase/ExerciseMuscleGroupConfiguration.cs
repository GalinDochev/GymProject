using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.SeedDatabase
{
    internal class ExerciseMuscleGroupConfiguration : IEntityTypeConfiguration<ExerciseMuscleGroup>
    {
        public void Configure(EntityTypeBuilder<ExerciseMuscleGroup> builder)
        {
            builder.HasKey(emg => new { emg.ExerciseId, emg.MuscleGroupId });

            builder.HasOne(emg => emg.Exercise).WithMany(ex => ex.ExerciseMuscleGroups).HasForeignKey(emg => emg.ExerciseId);

            builder.HasOne(emg => emg.MuscleGroup).WithMany(mg => mg.ExerciseMuscleGroups).HasForeignKey(emg => emg.MuscleGroupId);


            var data = new SeedData();
            builder.HasData(new ExerciseMuscleGroup[] { data.FirstExerciseMuscleGroup, data.SecondExerciseMuscleGroup, data.ThirdExerciseMuscleGroup, data.FourthExerciseMuscleGroup }
            );

        }
    }
}
