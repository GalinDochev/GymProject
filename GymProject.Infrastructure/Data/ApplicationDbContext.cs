﻿using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.SeedDatabase;
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
        public DbSet<ExerciseMuscleGroup> ExerciseMuscleGroup { get; set; }
        public DbSet<Exercise> Excercises { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Category> WorkoutCategories { get; set; }
        public DbSet<ExerciseWorkout> ExerciseWorkouts { get; set; }
        public DbSet<UserWorkout> UsersWorkouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var seedData = new SeedData();
            modelBuilder.ApplyConfiguration(new CategoryConfiguration(seedData));
            modelBuilder.ApplyConfiguration(new ExerciseConfiguration(seedData));
            modelBuilder.ApplyConfiguration(new ExerciseMuscleGroupConfiguration(seedData));
            modelBuilder.ApplyConfiguration(new MuscleGroupConfiguration(seedData));
            modelBuilder.ApplyConfiguration(new TrainerConfiguration(seedData));
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExerciseWorkout>()
                .HasKey(ew => new { ew.ExerciseId, ew.WorkoutId });

            modelBuilder.Entity<ExerciseWorkout>()
                .HasOne(ew => ew.Exercise)
                .WithMany(e => e.ExerciseWorkouts)
                .HasForeignKey(ew => ew.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserWorkout>()
                .HasKey(ep => new { ep.WorkoutId, ep.UserId });

            modelBuilder.Entity<UserWorkout>()
                .HasOne(ep => ep.Workout)
                .WithMany(e => e.UsersWorkouts)
                .HasForeignKey(ep => ep.WorkoutId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExerciseMuscleGroup>()
                .HasKey(ew => new { ew.ExerciseId, ew.MuscleGroupId });

            modelBuilder.Entity<ExerciseMuscleGroup>()
                .HasOne(ew => ew.Exercise)
                .WithMany(e => e.ExerciseMuscleGroups)
                .HasForeignKey(ew => ew.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}