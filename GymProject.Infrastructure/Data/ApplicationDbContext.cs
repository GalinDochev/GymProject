using GymProject.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymProject.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Excercises { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Category> WorkoutCategories { get; set; }
        public DbSet<ExerciseWorkout> ExerciseWorkouts { get; set; }
        public DbSet<UserWorkout> UsersWorkouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ExerciseWorkout>()
           .HasKey(ew => new { ew.ExerciseId, ew.WorkoutId });

            modelBuilder.Entity<ExerciseWorkout>()
                .HasOne(ew => ew.Exercise)
                .WithMany(e => e.ExerciseWorkouts)
                .HasForeignKey(ew => ew.ExerciseId);

            modelBuilder.Entity<ExerciseWorkout>()
                .HasOne(ew => ew.Workout)
                .WithMany(w => w.ExerciseWorkouts)
                .HasForeignKey(ew => ew.WorkoutId);


            modelBuilder.Entity<ExerciseMuscleGroup>()
        .HasKey(emg => new { emg.ExerciseId, emg.MuscleGroupId });

            modelBuilder.Entity<ExerciseMuscleGroup>()
                .HasOne(emg => emg.Exercise)
                .WithMany(ex => ex.ExerciseMuscleGroups)
                .HasForeignKey(emg => emg.ExerciseId);

            modelBuilder.Entity<ExerciseMuscleGroup>()
                .HasOne(emg => emg.MuscleGroup)
                .WithMany(mg => mg.ExerciseMuscleGroups)
                .HasForeignKey(emg => emg.MuscleGroupId);


            modelBuilder.Entity<UserWorkout>()
                .HasKey(ep => new { ep.WorkoutId, ep.UserId });

            modelBuilder.Entity<UserWorkout>()
           .HasOne(ep => ep.Workout)
           .WithMany(e => e.UsersWorkouts)
           .HasForeignKey(ep => ep.WorkoutId);
           
        }
    }

}