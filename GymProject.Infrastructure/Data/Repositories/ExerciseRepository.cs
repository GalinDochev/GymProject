﻿using GymProject.Infrastructure.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public class ExerciseRepository : Repository<Exercise>
    {
        private readonly ApplicationDbContext context;

        public ExerciseRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<Exercise>> GetAllNotDeleted()
        {
            var exercises = await context.Excercises
                .Where(e => e.IsDeleted == false)
                .Include(e => e.ExerciseMuscleGroups)
                    .ThenInclude(emg => emg.MuscleGroup)
                .Where(e => !e.ExerciseMuscleGroups.All(emg => emg.IsDeleted == true))
                .ToListAsync();

            return exercises;
        }

        public override async Task<Exercise> GetById(int id)
        {
            var exercise = await context.Excercises
                .Include(e => e.ExerciseMuscleGroups)
                    .ThenInclude(emg => emg.MuscleGroup)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                throw new Exception($"Exercise with id {id} is not found.");
            }
            
            return exercise;
        }

        public virtual async Task<List<Exercise>> GetExercisesByName(List<string> exerciseNames)
        {
            return await context.Excercises
                .Where(m => exerciseNames.Contains(m.Name))
                .ToListAsync();
        }

        public virtual async Task<List<string>> GetExercisesNames()
        {
           
                var exercisesNames = await context.Excercises.Select(ex => ex.Name).ToListAsync();
                return exercisesNames;
           
        }
    }
}
