using GymProject.Core.DTOs.ExerciseDTOs;
using GymProject.Core.Services;
using GymProject.Infrastructure.Data;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Services.Tests
{
    [TestFixture]
    public class ExerciseServiceUnitTests
    {
        private ExerciseService _exerciseService;
        private Mock<ExerciseRepository> _mockExerciseRepository;
        private Mock<ExerciseMuscleGroupRepository> _mockExerciseMuscleGroupRepository;
        private Mock<MuscleGroupRepository> _mockMuscleGroupRepository;
        private Mock<ExerciseWorkoutRepository> _mockExerciseWorkoutRepository;
        private Mock<ILogger<ExerciseService>> _mockLogger;
        private DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext _dbContext;
        [OneTimeSetUp]
        public void Setup()
        {
            _mockExerciseRepository = new Mock<ExerciseRepository>(_dbContext);
            _mockExerciseMuscleGroupRepository = new Mock<ExerciseMuscleGroupRepository>(_dbContext);
            _mockMuscleGroupRepository = new Mock<MuscleGroupRepository>(_dbContext);
            _mockExerciseWorkoutRepository = new Mock<ExerciseWorkoutRepository>(_dbContext);
            _mockLogger = new Mock<ILogger<ExerciseService>>();

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            var dbContext = new ApplicationDbContext(_options);

            _exerciseService = new ExerciseService(
                _mockExerciseRepository.Object,
                _mockExerciseMuscleGroupRepository.Object,
                _mockMuscleGroupRepository.Object,
                _mockExerciseWorkoutRepository.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task GetAllNotDeletedExForTrainers_ReturnsExercises()
        {
            var exercises = new List<Exercise>
        {
            new Exercise { Id = 1, Name = "Exercise 1" },
            new Exercise { Id = 2, Name = "Exercise 2" }
        };
            _mockExerciseRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(exercises);

            var result = await _exerciseService.GetAllNotDeletedExForTrainers();

            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Exercise 1"));
            Assert.That(result[0].Name, Is.EqualTo("Exercise 1"));
            Assert.That(result[1].Name, Is.EqualTo("Exercise 2"));
        }
        [Test]
        public async Task GetAllNotDeletedExercises_ReturnsExercises()
        {
            var exercises = new List<Exercise>
                    {
                        new Exercise { Id = 1, Name = "Exercise 1", DifficultyLevel = 3, ImageUrl = "image1.jpg", Repetitions = 10, Sets = 3, IsDeleted = false },
                        new Exercise { Id = 2, Name = "Exercise 2", DifficultyLevel = 5, ImageUrl = "image2.jpg", Repetitions = 12, Sets = 4, IsDeleted = false }
                    };
            _mockExerciseRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(exercises);

            var result = await _exerciseService.GetAllNotDeletedExercises();

            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var firstExercise = result.FirstOrDefault();
            Assert.IsNotNull(firstExercise);
            Assert.That(firstExercise.Id, Is.EqualTo(1));
            Assert.That(firstExercise.Name, Is.EqualTo("Exercise 1"));
            Assert.That(firstExercise.DifficultyLevel, Is.EqualTo(3));
            Assert.That(firstExercise.ImageUrl, Is.EqualTo("image1.jpg"));
            Assert.That(firstExercise.Repetitions, Is.EqualTo(10));
            Assert.That(firstExercise.Sets, Is.EqualTo(3));
            Assert.That(firstExercise.DeleteTime, Is.EqualTo(default(DateTime)));
            Assert.IsFalse(firstExercise.IsDeleted);
            Assert.IsEmpty(firstExercise.MuscleGroups);

            var secondExercise = result.Skip(1).FirstOrDefault();
            Assert.That(secondExercise, Is.Not.Null);
            Assert.That(secondExercise.Id, Is.EqualTo(2));
            Assert.That(secondExercise.Name, Is.EqualTo("Exercise 2"));
            Assert.That(secondExercise.DifficultyLevel, Is.EqualTo(5));
            Assert.That(secondExercise.ImageUrl, Is.EqualTo("image2.jpg"));
            Assert.That(secondExercise.Repetitions, Is.EqualTo(12));
            Assert.That(secondExercise.Sets, Is.EqualTo(4));
            Assert.That(secondExercise.DeleteTime, Is.EqualTo(default(DateTime)));
            Assert.IsFalse(secondExercise.IsDeleted);
            Assert.IsEmpty(secondExercise.MuscleGroups);
        }
        [Test]
        public async Task GetExerciseByIdForDetails_ReturnsExerciseForDetails()
        {
            int exerciseId = 1;
            var exercise = new Exercise
            {
                Id = exerciseId,
                Name = "Test Exercise",
                Description = "Exercise Description",
                DifficultyLevel = 3,
                ImageUrl = "image.jpg",
                Repetitions = 10,
                Sets = 3,
                ExerciseMuscleGroups = new List<ExerciseMuscleGroup>
            {
                new ExerciseMuscleGroup { MuscleGroup = new MuscleGroup { Name = "Muscle Group 1" } },
                new ExerciseMuscleGroup { MuscleGroup = new MuscleGroup { Name = "Muscle Group 2" } }
            }
            };
            _mockExerciseRepository.Setup(repo => repo.GetById(exerciseId)).ReturnsAsync(exercise);

            var result = await _exerciseService.GetExerciseByIdForDetails(exerciseId);

            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(exerciseId));
            Assert.That(result.Name, Is.EqualTo("Test Exercise"));
            Assert.That(result.Description, Is.EqualTo("Exercise Description"));
            Assert.That(result.DifficultyLevel, Is.EqualTo(3));
            Assert.That(result.ImageUrl, Is.EqualTo("image.jpg"));
            Assert.That(result.Repetitions, Is.EqualTo(10));
            Assert.That(result.Sets, Is.EqualTo(3));
            Assert.That(result.MuscleGroups.Count, Is.EqualTo(2));
            Assert.Contains("Muscle Group 1", result.MuscleGroups);
            Assert.Contains("Muscle Group 2", result.MuscleGroups);
        }
        [Test]
        public async Task GetExerciseDTOModel_ReturnsExerciseDTOModel()
        {
            var muscleGroups = new List<MuscleGroup>
        {
            new MuscleGroup { Id = 1, Name = "Muscle Group 1" },
            new MuscleGroup { Id = 2, Name = "Muscle Group 2" }
        };
            _mockMuscleGroupRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(muscleGroups);

            var result = await _exerciseService.GetExerciseDTOModel();

            Assert.NotNull(result);
            Assert.That(result.SelectedMuslceGroups.Count, Is.EqualTo(2));
            Assert.IsTrue(result.SelectedMuslceGroups.Any(g => g.Name == "Muscle Group 1"));
            Assert.IsTrue(result.SelectedMuslceGroups.Any(g => g.Name == "Muscle Group 2"));
        }

        [Test]
        public async Task GetMuscleGroupsNames_ReturnsMuscleGroupNames()
        {
            var expectedMuscleGroups = new List<string> { "Muscle Group 1", "Muscle Group 2" };
            _mockMuscleGroupRepository.Setup(repo => repo.GetMuscleGroupsNames()).ReturnsAsync(expectedMuscleGroups);

            var result = await _exerciseService.GetMuscleGroupsNames();

            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(expectedMuscleGroups.Count));
            Assert.That(result, Is.EqualTo(expectedMuscleGroups));
        }

        [Test]
        public async Task GetExercisesNames_ReturnsExerciseNames()
        {
            var expectedExerciseNames = new List<string> { "Exercise 1", "Exercise 2" };
            _mockExerciseRepository.Setup(repo => repo.GetExercisesNames()).ReturnsAsync(expectedExerciseNames);

            var result = await _exerciseService.GetExercisesNames();

            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(expectedExerciseNames.Count));
            Assert.That(result, Is.EqualTo(expectedExerciseNames));
        }
        [Test]
        public async Task GetMuscleGroupsByName_ReturnsMuscleGroups()
        {
            var muscleGroupNames = new List<string> { "MuscleGroup1", "MuscleGroup2" };
            var expectedMuscleGroups = new List<MuscleGroup>
                {
                    new MuscleGroup { Name = "MuscleGroup1" },
                    new MuscleGroup { Name = "MuscleGroup2" }
                };
            _mockMuscleGroupRepository.Setup(repo => repo.GetMuscleGroupsByName(muscleGroupNames)).ReturnsAsync(expectedMuscleGroups);

            var result = await _exerciseService.GetMuscleGroupsByName(muscleGroupNames);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedMuscleGroups.Count));
            Assert.That(result, Is.EqualTo(expectedMuscleGroups));
        }

        [Test]
        public async Task GetExercisesByName_ReturnsExercises()
        {
            var exerciseNames = new List<string> { "Exercise1", "Exercise2" };
            var expectedExercises = new List<Exercise>
        {
            new Exercise { Name = "Exercise1" },
            new Exercise { Name = "Exercise2" }
        };
            _mockExerciseRepository.Setup(repo => repo.GetExercisesByName(exerciseNames)).ReturnsAsync(expectedExercises);

            var result = await _exerciseService.GetExercisesByName(exerciseNames);

            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(expectedExercises.Count));
            Assert.That(result, Is.EqualTo(expectedExercises));
        }
        [Test]
        public async Task AddExercise_Successfully()
        {
            var exerciseDto = new AddExerciseDTO
            {
                Id = 1,
                Description = "Test Description",
                DifficultyLevel = 3,
                Name = "Test Exercise",
                Sets = 3,
                Repetitions = 10,
                ImageUrl = "test.jpg",
                SelectedMuslceGroups = new List<MuscleGroup> { new MuscleGroup { Id = 1, Name = "MuscleGroup1" } }
            };

            _mockExerciseRepository.Setup(repo => repo.Add(It.IsAny<Exercise>())).Verifiable();

            await _exerciseService.AddExercise(exerciseDto);

            _mockExerciseRepository.Verify(repo => repo.Add(It.IsAny<Exercise>()), Times.Once);
        }
        [Test]
        public async Task EditExercise_Successfully()
        {
            var exerciseDto = new AddExerciseDTO
            {
                Id = 1,
                Description = "Updated Description",
                DifficultyLevel = 4,
                Name = "Updated Exercise",
                Sets = 4,
                Repetitions = 12,
                ImageUrl = "updated.jpg",
                SelectedMuslceGroups = new List<MuscleGroup> { new MuscleGroup { Id = 1, Name = "MuscleGroup1" } }
            };

            var existingExercise = new Exercise
            {
                Id = exerciseDto.Id,
                Description = "Old Description",
                DifficultyLevel = 3,
                Name = "Old Exercise",
                Sets = 3,
                Repetitions = 10,
                ImageUrl = "old.jpg",
                ExerciseMuscleGroups = new List<ExerciseMuscleGroup>
                    {
                        new ExerciseMuscleGroup { MuscleGroupId = 1, IsDeleted = false }
                    }
            };

            _mockExerciseRepository.Setup(repo => repo.GetById(exerciseDto.Id)).ReturnsAsync(existingExercise);
            _mockExerciseMuscleGroupRepository.Setup(repo => repo.Delete(It.IsAny<ExerciseMuscleGroup>())).Returns(Task.CompletedTask);
            _mockExerciseMuscleGroupRepository.Setup(repo => repo.Update(It.IsAny<ExerciseMuscleGroup>())).Returns(Task.CompletedTask);

            await _exerciseService.EditExercise(exerciseDto);
        }
        [Test]
        public async Task DeleteExercise_Successfully()
        {
            int exerciseId = 1;

            var exerciseToDelete = new Exercise
            {
                Id = exerciseId,
                ExerciseMuscleGroups = new List<ExerciseMuscleGroup>(),
                ExerciseWorkouts = new List<ExerciseWorkout>()
            };

            _mockExerciseRepository.Setup(repo => repo.GetById(exerciseId)).ReturnsAsync(exerciseToDelete);
            _mockExerciseMuscleGroupRepository.Setup(repo => repo.Delete(It.IsAny<ExerciseMuscleGroup>())).Returns(Task.CompletedTask);
            _mockExerciseWorkoutRepository.Setup(repo => repo.Delete(It.IsAny<ExerciseWorkout>())).Returns(Task.CompletedTask);
            _mockExerciseRepository.Setup(repo => repo.Delete(It.IsAny<Exercise>())).Returns(Task.CompletedTask);

            await _exerciseService.DeleteExercise(exerciseId);

            _mockExerciseRepository.Verify(repo => repo.GetById(exerciseId), Times.Once);
            _mockExerciseMuscleGroupRepository.Verify(repo => repo.Delete(It.IsAny<ExerciseMuscleGroup>()), Times.Exactly(exerciseToDelete.ExerciseMuscleGroups.Count));
            _mockExerciseWorkoutRepository.Verify(repo => repo.Delete(It.IsAny<ExerciseWorkout>()), Times.Exactly(exerciseToDelete.ExerciseWorkouts.Count));
            _mockExerciseRepository.Verify(repo => repo.Delete(It.IsAny<Exercise>()), Times.Once);
        }
    }
}
