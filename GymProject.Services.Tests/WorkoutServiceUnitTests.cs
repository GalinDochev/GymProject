using GymProject.Core.Services;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using GymProject.Infrastructure.Data;
using GymProject.Core.DTOs.WorkoutDTOs;
using GymProject.Core.Exceptions;

namespace GymProject.Services.Tests
{
    public class WorkoutServiceUnitTests
    {
        [TestFixture]
        public class WorkoutServiceTests
        {
            private Mock<WorkoutRepository> _mockWorkoutRepository;
            private DbContextOptions<ApplicationDbContext> _options;
            private ApplicationDbContext _dbContext;
            private Mock<UserWorkoutRepository> _mockUserWorkoutRepository;
            private Mock<ExerciseRepository> _mockExerciseRepository;
            private Mock<CategoryRepository> _mockCategoryRepository;
            private Mock<ExerciseWorkoutRepository> _mockExerciseWorkoutRepository;
            private Mock<ILogger<WorkoutService>> _mockLogger;
            private WorkoutService _workoutService;

            [OneTimeSetUp]
            public void SetUp()
            {
                _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "TestDatabase")
                 .Options;

                _dbContext = new ApplicationDbContext(_options);

                _mockLogger = new Mock<ILogger<WorkoutService>>();
                _mockWorkoutRepository = new Mock<WorkoutRepository>(_dbContext);
                _mockUserWorkoutRepository = new Mock<UserWorkoutRepository>(_dbContext);
                _mockExerciseRepository = new Mock<ExerciseRepository>(_dbContext);
                _mockCategoryRepository = new Mock<CategoryRepository>(_dbContext);
                _mockExerciseWorkoutRepository = new Mock<ExerciseWorkoutRepository>(_dbContext);

                _workoutService = new WorkoutService(
                    _mockWorkoutRepository.Object,
                    _mockUserWorkoutRepository.Object,
                    _mockExerciseRepository.Object,
                    _mockCategoryRepository.Object,
                    _mockExerciseWorkoutRepository.Object,
                    _mockLogger.Object
                );
            }

            [Test]
            public async Task GetAllNotDeletedWorkouts_ReturnsAllNotDeletedWorkouts()
            {

                var expectedWorkouts = new List<Workout>
              {
                  new Workout { Id = 1, Name = "Workout 1", DifficultyLevel = 3, Duration = 45, CategoryId = 1, CreatorId = "user1" },
                  new Workout { Id = 2, Name = "Workout 2", DifficultyLevel = 4, Duration = 30, CategoryId = 2, CreatorId = "user2" },
                  new Workout { Id = 3, Name = "Workout 3", DifficultyLevel = 5, Duration = 60, CategoryId = 3, CreatorId = "user3" }
              };
                _mockWorkoutRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(expectedWorkouts);

                var result = await _workoutService.GetAllNotDeletedWorkouts();

                Assert.That(result.Count, Is.EqualTo(expectedWorkouts.Count));
                Assert.That(result[0].Name, Is.EqualTo(expectedWorkouts[0].Name));
                Assert.That(result[1].DifficultyLevel, Is.EqualTo(expectedWorkouts[1].DifficultyLevel));
                Assert.That(result[2].Duration, Is.EqualTo(expectedWorkouts[2].Duration));
                Assert.That(result[2].CategoryId, Is.EqualTo(expectedWorkouts[2].CategoryId));
                Assert.That(result[2].Id, Is.EqualTo(expectedWorkouts[2].Id));
                Assert.That(result[2].CreatorId, Is.EqualTo(expectedWorkouts[2].CreatorId));
            }

            [Test]
            public async Task GetAllNotDeletedWorkouts_ReturnsEmptyListWhenNoWorkouts()
            {
                var expectedWorkouts = new List<Workout>();
                _mockWorkoutRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(expectedWorkouts);

                var result = await _workoutService.GetAllNotDeletedWorkouts();


                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(0));
            }

            [Test]
            public async Task GetWorkoutByIdForDetails_ReturnsWorkoutForDetails()
            {
                // Arrange
                int workoutId = 1;
                var expectedWorkout = new Workout
                {
                    Id = workoutId,
                    Name = "Test Workout",
                    Description = "Test Description",
                    DifficultyLevel = 3,
                    ImageUrl = "test.jpg",
                    Category = new Category { Id = 1, IsDeleted = false, Name = "testCategory" },
                    CreatorId = "creatorId",
                    CategoryId = 1,
                    Duration = 60,
                    ExerciseWorkouts = new List<ExerciseWorkout>
        {
            new ExerciseWorkout { WorkoutId = workoutId, ExerciseId = 1, IsDeleted = false },
            new ExerciseWorkout { WorkoutId = workoutId, ExerciseId = 2, IsDeleted = false }
        }
                };

                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutId)).ReturnsAsync(expectedWorkout);

                var result = await _workoutService.GetWorkoutByIdForDetails(workoutId);

                Assert.That(result.Id, Is.EqualTo(expectedWorkout.Id));
                Assert.That(result.Name, Is.EqualTo(expectedWorkout.Name));
                Assert.That(result.Description, Is.EqualTo(expectedWorkout.Description));
                Assert.That(result.DifficultyLevel, Is.EqualTo(expectedWorkout.DifficultyLevel));
                Assert.That(result.ImageUrl, Is.EqualTo(expectedWorkout.ImageUrl));
                Assert.That(result.Creator, Is.EqualTo(expectedWorkout.Creator));
                Assert.That(result.Category, Is.EqualTo(expectedWorkout.Category));
                Assert.That(result.CreatorId, Is.EqualTo(expectedWorkout.CreatorId));
                Assert.That(result.CategoryId, Is.EqualTo(expectedWorkout.CategoryId));
                Assert.That(result.Duration, Is.EqualTo(expectedWorkout.Duration));
                Assert.That(result.ExerciseWorkouts.Count, Is.EqualTo(expectedWorkout.ExerciseWorkouts.Count));
            }

            [Test]
            public async Task JoinWorkout_SuccessfullyJoinsWorkoutThatHeHasntJoined()
            {
                int workoutId = 1;
                string userId = "user123";

                var workout = new Workout
                {
                    Id = workoutId,
                    CreatorId = "creator123",
                    UsersWorkouts = new List<UserWorkout> { new UserWorkout { UserId = "creator123", WorkoutId = workoutId } }
                };

                var userWorkout = new UserWorkout
                {
                    UserId = userId,
                    WorkoutId = workoutId,
                    IsDeleted = false
                };

                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutId)).ReturnsAsync(workout);
                _mockUserWorkoutRepository.Setup(repo => repo.GetByUserIdAndWorkoutId(userId, workoutId)).ReturnsAsync((UserWorkout)null);

                await _workoutService.JoinWorkout(workoutId, userId);

                Assert.That(workout.UsersWorkouts.Count, Is.EqualTo(2));
                Assert.That(workout.UsersWorkouts.Any(uw => uw.UserId == userId && uw.WorkoutId == workoutId && !uw.IsDeleted));
            }

            [Test]
            public async Task JoinWorkout_SuccessfullyJoinsWorkoutThatHeHasJoinedPreviously()
            {
                int workoutId = 1;
                string userId = "user123";

                var workout = new Workout
                {
                    Id = workoutId,
                    CreatorId = "creator123",
                    UsersWorkouts = new List<UserWorkout> { new UserWorkout { UserId = "creator123", WorkoutId = workoutId } }
                };

                var userWorkout = new UserWorkout
                {
                    UserId = userId,
                    WorkoutId = workoutId,
                    IsDeleted = true
                };

                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutId)).ReturnsAsync(workout);
                _mockUserWorkoutRepository.Setup(repo => repo.GetByUserIdAndWorkoutId(userId, workoutId)).ReturnsAsync(userWorkout);

                await _workoutService.JoinWorkout(workoutId, userId);

                _mockUserWorkoutRepository.Verify(repo => repo.Update(userWorkout), Times.Once);
                Assert.IsFalse(userWorkout.IsDeleted);
                Assert.That(workout.UsersWorkouts.Count, Is.EqualTo(2));
            }

            [Test]
            public async Task JoinWorkout_CannotJoinCreatedWorkout()
            {
                // Arrange
                int workoutId = 1;
                string userId = "creator123";

                var workout = new Workout
                {
                    Id = workoutId,
                    CreatorId = userId
                };

                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutId)).ReturnsAsync(workout);

                // Act & Assert
                var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _workoutService.JoinWorkout(workoutId, userId));
                Assert.That(ex.Message, Is.EqualTo("You cannot join a workout that you created."));
            }
            [Test]
            public async Task RemoveWorkoutFromCollectionAsync_ThrowsExceptionWhenUserIsCreator()
            {
                // Arrange
                int workoutId = 1;
                string userId = "creator123";

                var workout = new Workout
                {
                    Id = workoutId,
                    CreatorId = userId
                };

                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutId)).ReturnsAsync(workout);

                // Act & Assert
                var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _workoutService.RemoveWorkoutFromCollectionAsync(workoutId, userId));
                Assert.That(ex.Message, Is.EqualTo("You cannot leave a workout that you created."));
            }

            [Test]
            public async Task RemoveWorkoutFromCollectionAsync_RemovesWorkoutFromCollection()
            {
                // Arrange
                int workoutId = 1;
                string userId = "user123";

                var workout = new Workout
                {
                    Id = workoutId,
                    CreatorId = "creator123",
                    UsersWorkouts = new List<UserWorkout>
        {
            new UserWorkout { WorkoutId = workoutId, UserId = "creator123" }, // UserWorkout for creator
            new UserWorkout { WorkoutId = workoutId, UserId = userId } // UserWorkout for specified user
        }
                };

                var userWorkout = new UserWorkout
                {
                    UserId = userId,
                    WorkoutId = workoutId,
                    IsDeleted = false
                };

                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutId)).ReturnsAsync(workout);
                _mockUserWorkoutRepository.Setup(repo => repo.GetByUserIdAndWorkoutId(userId, workoutId)).ReturnsAsync(userWorkout);

                // Act
                Console.WriteLine($"Before removal: UsersWorkouts count = {workout.UsersWorkouts.Count}");
                await _workoutService.RemoveWorkoutFromCollectionAsync(workoutId, userId);
                Console.WriteLine($"After removal: UsersWorkouts count = {workout.UsersWorkouts.Count}");

                // Assert
                Assert.That(workout.UsersWorkouts.Count, Is.EqualTo(2));

            }

            [Test]
            public async Task GetWorkoutDTOModel_ReturnsCorrectModel()
            {
                // Arrange
                var mockExercises = new List<Exercise>
                     {
                          new Exercise { Id = 1, Name = "Exercise 1", IsDeleted = false },
                          new Exercise { Id = 2, Name = "Exercise 2", IsDeleted = false }
                     };

                _mockExerciseRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(mockExercises);

                var mockCategories = new List<Category>
                     {
                          new Category { Id = 1, Name = "Category 1", IsDeleted = false },
                          new Category { Id = 2, Name = "Category 2", IsDeleted = false }
                     };

                _mockCategoryRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(mockCategories);

                // Act
                var result = await _workoutService.GetWorkoutDTOModel();

                // Assert
                Assert.NotNull(result);
                Assert.IsInstanceOf<AddWorkoutDTO>(result);
                Assert.That(result.SelectedExercises.Count, Is.EqualTo(mockExercises.Count));
                Assert.That(result.SelectedCategories.Count, Is.EqualTo(mockCategories.Count));

                foreach (var exercise in mockExercises)
                {
                    Assert.IsTrue(result.SelectedExercises.Any(e => e.Id == exercise.Id && e.Name == exercise.Name));
                }

                foreach (var category in mockCategories)
                {
                    Assert.IsTrue(result.SelectedCategories.Any(c => c.Id == category.Id && c.Name == category.Name));
                }
            }

            [Test]
            public async Task GetCategoryByName_ReturnsCategory()
            {
                // Arrange
                string categoryName = "TestCategory";
                var expectedCategory = new Category { Name = categoryName };

                _mockCategoryRepository.Setup(repo => repo.GetCategoryByName(categoryName)).ReturnsAsync(expectedCategory);

                // Act
                var result = await _workoutService.GetCategoryByName(categoryName);

                // Assert
                Assert.NotNull(result);
                Assert.That(result, Is.EqualTo(expectedCategory));
            }
            [Test]
            public async Task GetCategoriesNames_ReturnsListOfCategoryNames()
            {
                // Arrange
                var categories = new List<Category>
                    {
                        new Category { Name = "Category1" },
                        new Category { Name = "Category2" },
                        new Category { Name = "Category3" }
                    };

                _mockCategoryRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(categories);

                // Act
                var result = await _workoutService.GetCategoriesNames();

                // Assert
                Assert.NotNull(result);
                Assert.That(result, Is.InstanceOf<List<string>>());
                Assert.That(result.Count, Is.EqualTo(categories.Count));

                foreach (var category in categories)
                {
                    Assert.Contains(category.Name, result);
                }
            }

            public async Task AddWorkout_SuccessfullyAddsWorkout()
            {
                // Arrange
                var workoutDTO = new AddWorkoutDTO
                {
                    Id = 1,
                    Description = "Workout description",
                    DifficultyLevel = 1,
                    Name = "Workout Name",
                    ImageUrl = "workout_image_url",
                    Duration = 60,
                    Category = new Category { Id = 1, Name = "Category Name" },
                    CreatorId = "creator123",
                    SelectedExercises = new List<Exercise>
                        {
                            new Exercise { Id = 1, Name = "Exercise 1" },
                            new Exercise { Id = 2, Name = "Exercise 2" }
                        }
                };

                _mockWorkoutRepository.Setup(repo => repo.Add(It.IsAny<Workout>())).Verifiable();

                // Act
                await _workoutService.AddWorkout(workoutDTO);

                // Assert
                _mockWorkoutRepository.Verify(repo => repo.Add(It.IsAny<Workout>()), Times.Once);
            }
            [Test]
            public void AddWorkout_ThrowsException_WhenInvalidOperation()
            {
                // Arrange
                var workoutDTO = new AddWorkoutDTO();

                var ex = Assert.ThrowsAsync<NullReferenceException>(() => _workoutService.AddWorkout(workoutDTO));
                Assert.That(ex.Message, Is.EqualTo("Object reference not set to an instance of an object."));
            }

            [Test]
            public async Task EditWorkout_ThrowsException_WhenUnauthorizedAndNotAdmin()
            {
                // Arrange
                var workoutDTO = new AddWorkoutDTO
                {
                };
                bool isAdmin = false;

                var workoutToEdit = new Workout { CreatorId = "differentCreatorId" };
                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutDTO.Id)).ReturnsAsync(workoutToEdit);

                // Act & Assert
                var ex = Assert.ThrowsAsync<Exception>(() => _workoutService.EditWorkout(workoutDTO, isAdmin));
                Assert.That(ex.Message, Is.EqualTo("You cannot edit a workout that you havent created"));
            }

            [Test]
            public async Task EditWorkout_UpdatesWorkoutAndAssociations_WhenAuthorized()
            {
                var workoutDTO = new AddWorkoutDTO
                {
                    Id = 1,
                    CreatorId = "creator123",
                    Name = "New Workout Name",
                    CategoryId = 1,

                };
                bool isAdmin = true;

                var workoutToEdit = new Workout
                {
                    Id = workoutDTO.Id,
                    CreatorId = workoutDTO.CreatorId,
                    ExerciseWorkouts = new List<ExerciseWorkout> 
                    {
                        new ExerciseWorkout { ExerciseId = 1, WorkoutId = workoutDTO.Id },
                        new ExerciseWorkout { ExerciseId = 2, WorkoutId = workoutDTO.Id } }
                };
                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutDTO.Id)).ReturnsAsync(workoutToEdit);

                var existingExerciseWorkouts = workoutToEdit.ExerciseWorkouts.ToList();

                _mockExerciseWorkoutRepository.Setup(repo => repo.Delete(It.IsAny<ExerciseWorkout>())).Returns(Task.CompletedTask);
                _mockExerciseWorkoutRepository.Setup(repo => repo.Add(It.IsAny<ExerciseWorkout>())).Returns(Task.CompletedTask);
                _mockExerciseWorkoutRepository.Setup(repo => repo.Update(It.IsAny<ExerciseWorkout>())).Returns(Task.CompletedTask);

                // Act
                await _workoutService.EditWorkout(workoutDTO, isAdmin);

                // Assert

                Assert.That(workoutToEdit.Name, Is.EqualTo(workoutDTO.Name));
                // Add assertions for other properties as needed

                foreach (var exerciseDTO in workoutDTO.SelectedExercises)
                {
                    var existingExerciseWorkout = existingExerciseWorkouts.FirstOrDefault(ex => ex.ExerciseId == exerciseDTO.Id);
                    if (existingExerciseWorkout != null)
                    {
                        Assert.IsFalse(existingExerciseWorkout.IsDeleted);
                    }
                    else
                    {
                        _mockExerciseWorkoutRepository.Verify(repo => repo.Add(It.Is<ExerciseWorkout>(ex => ex.ExerciseId == exerciseDTO.Id)), Times.Once);
                    }
                }

            }

            [Test]
            public async Task DeleteWorkout_DeletesWorkout_WhenCreatedByUser()
            {
                int workoutId = 1;
                string userId = "creator123";
                bool isAdmin = false;

                var workoutToDelete = new Workout { Id = workoutId, CreatorId = userId };
                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutId)).ReturnsAsync(workoutToDelete);

                await _workoutService.DeleteWorkout(workoutId, userId, isAdmin);

                _mockExerciseWorkoutRepository.Verify(repo => repo.Delete(It.IsAny<ExerciseWorkout>()), Times.Exactly(workoutToDelete.ExerciseWorkouts.Count));
                _mockUserWorkoutRepository.Verify(repo => repo.Delete(It.IsAny<UserWorkout>()), Times.Exactly(workoutToDelete.UsersWorkouts.Count));
                _mockWorkoutRepository.Verify(repo => repo.Delete(workoutToDelete), Times.Once);
            }

            [Test]
            public async Task DeleteWorkout_DeletesWorkout_WhenAdmin()
            {
                int workoutId = 1;
                string userId = "user123";
                bool isAdmin = true;

                var workoutToDelete = new Workout { Id = workoutId, CreatorId = "creator123" };
                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutId)).ReturnsAsync(workoutToDelete);

                await _workoutService.DeleteWorkout(workoutId, userId, isAdmin);

                _mockExerciseWorkoutRepository.Verify(repo => repo.Delete(It.IsAny<ExerciseWorkout>()), Times.Exactly(workoutToDelete.ExerciseWorkouts.Count));
                _mockUserWorkoutRepository.Verify(repo => repo.Delete(It.IsAny<UserWorkout>()), Times.Exactly(workoutToDelete.UsersWorkouts.Count));
                _mockWorkoutRepository.Verify(repo => repo.Delete(workoutToDelete), Times.Once);
            }

            [Test]
            public async Task DeleteWorkout_ThrowsException_WhenNotAuthorized()
            {
                int workoutId = 1;
                string userId = "user123";
                bool isAdmin = false;

                var workoutToDelete = new Workout { Id = workoutId, CreatorId = "creator123" };
                _mockWorkoutRepository.Setup(repo => repo.GetById(workoutId)).ReturnsAsync(workoutToDelete);

                var ex =  Assert.ThrowsAsync<UnAuthorizedActionException>(() => _workoutService.DeleteWorkout(workoutId, userId, isAdmin));
                Assert.That(ex.Message, Is.EqualTo("You cant Delete a workout that you havent created"));
            }

            [Test]
            public void ApplySearchFilter_ReturnsOriginalList_WhenWorkoutsDTOsIsNull()
            {
                List<WorkoutCardDTO> workoutsDTOs = null;
                string searchString = "search";

                var ex = Assert.Throws<ArgumentNullException>(() => _workoutService.ApplySearchFilter(workoutsDTOs, searchString));
                Assert.That(ex.ParamName, Is.EqualTo("workoutsDTOs"));
            }

            [Test]
            public void ApplySearchFilter_ReturnsOriginalList_WhenSearchStringIsNullOrEmpty()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>();
                string searchString = null;

                // Act
                var result = _workoutService.ApplySearchFilter(workoutsDTOs, searchString);

                // Assert
                Assert.That(result, Is.EqualTo(workoutsDTOs));
            }

            [Test]
            public void ApplySearchFilter_ReturnsFilteredWorkouts_WhenSearchStringMatches()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>
                    {
                        new WorkoutCardDTO { Name = "Workout A" },
                        new WorkoutCardDTO { Name = "Workout B" },
                        new WorkoutCardDTO { Name = "Other Workout" }
                    };
                string searchString = "Workout";

                // Act
                var result = _workoutService.ApplySearchFilter(workoutsDTOs, searchString);

                // Assert
                Assert.That(result.Count, Is.EqualTo(3));
                Assert.IsTrue(result.All(w => w.Name.ToLower().Contains(searchString.ToLower())));
            }

            [Test]
            public void ApplySearchFilter_ReturnsEmptyList_WhenSearchStringDoesNotMatch()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>
                    {
                        new WorkoutCardDTO { Name = "Workout A" },
                        new WorkoutCardDTO { Name = "Workout B" },
                        new WorkoutCardDTO { Name = "Other Workout" }
                    };
                string searchString = "Nonexistent";

                // Act
                var result = _workoutService.ApplySearchFilter(workoutsDTOs, searchString);

                // Assert
                Assert.IsEmpty(result);
            }

            [Test]
            public void ApplyCategoryFilter_ReturnsOriginalList_WhenWorkoutsDTOsIsNull()
            {
                // Arrange
                List<WorkoutCardDTO> workoutsDTOs = null;
                string category = "SomeCategory";

                // Act & Assert
                var ex = Assert.Throws<ArgumentNullException>(() => _workoutService.ApplyCategoryFilter(workoutsDTOs, category));
                Assert.That(ex.ParamName, Is.EqualTo("workoutsDTOs"));
            }

            [Test]
            public void ApplyCategoryFilter_ReturnsOriginalList_WhenCategoryIsNullOrEmpty()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>();
                string category = null;

                // Act
                var result = _workoutService.ApplyCategoryFilter(workoutsDTOs, category);

                // Assert
                Assert.That(result, Is.EqualTo(workoutsDTOs));
            }

            [Test]
            public void ApplyCategoryFilter_ReturnsFilteredWorkouts_WhenCategoryMatches()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>
                    {
                        new WorkoutCardDTO { Name = "Workout A", Category = new Category { Name = "Category A" } },
                        new WorkoutCardDTO { Name = "Workout B", Category = new Category { Name = "Category B" } },
                        new WorkoutCardDTO { Name = "Workout C", Category = new Category { Name = "Category A" } }
                    };
                string category = "Category A";

                // Act
                var result = _workoutService.ApplyCategoryFilter(workoutsDTOs, category);

                // Assert
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.IsTrue(result.All(w => w.Category?.Name == category));
            }

            [Test]
            public void ApplyCategoryFilter_ReturnsEmptyList_WhenCategoryDoesNotMatch()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>
                    {
                        new WorkoutCardDTO { Name = "Workout A", Category = new Category { Name = "Category A" } },
                        new WorkoutCardDTO { Name = "Workout B", Category = new Category { Name = "Category B" } },
                        new WorkoutCardDTO { Name = "Workout C", Category = new Category { Name = "Category A" } }
                    };
                string category = "Nonexistent Category";

                // Act
                var result = _workoutService.ApplyCategoryFilter(workoutsDTOs, category);

                // Assert
                Assert.IsEmpty(result);
            }

            [Test]
            public void ApplyDifficultyLevelFilter_ReturnsOriginalList_WhenWorkoutsDTOsIsNull()
            {
                // Arrange
                List<WorkoutCardDTO> workoutsDTOs = null;
                string difficultyLevelGroup = "1-3";

                // Act & Assert
                var ex = Assert.Throws<ArgumentNullException>(() => _workoutService.ApplyDifficultyLevelFilter(workoutsDTOs, difficultyLevelGroup));
                Assert.That(ex.ParamName, Is.EqualTo("workoutsDTOs"));
            }

            [Test]
            public void ApplyDifficultyLevelFilter_ReturnsOriginalList_WhenDifficultyLevelGroupIsNullOrEmpty()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>();
                string difficultyLevelGroup = null;

                // Act
                var result = _workoutService.ApplyDifficultyLevelFilter(workoutsDTOs, difficultyLevelGroup);

                // Assert
                Assert.That(result, Is.EqualTo(workoutsDTOs));
            }

            [Test]
            public void ApplyDifficultyLevelFilter_ReturnsFilteredWorkouts_WhenDifficultyLevelGroupIs1to3()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>
                    {
                        new WorkoutCardDTO { Name = "Workout A", DifficultyLevel = 1 },
                        new WorkoutCardDTO { Name = "Workout B", DifficultyLevel = 2 },
                        new WorkoutCardDTO { Name = "Workout C", DifficultyLevel = 3 },
                        new WorkoutCardDTO { Name = "Workout D", DifficultyLevel = 4 }
                    };
                string difficultyLevelGroup = "1-3";

                // Act
                var result = _workoutService.ApplyDifficultyLevelFilter(workoutsDTOs, difficultyLevelGroup);

                // Assert
                Assert.That(result.Count, Is.EqualTo(3));
                Assert.IsTrue(result.All(w => w.DifficultyLevel >= 1 && w.DifficultyLevel <= 3));
            }

            [Test]
            public void ApplyDifficultyLevelFilter_ReturnsFilteredWorkouts_WhenDifficultyLevelGroupIs4to7()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>
                    {
                        new WorkoutCardDTO { Name = "Workout A", DifficultyLevel = 1 },
                        new WorkoutCardDTO { Name = "Workout B", DifficultyLevel = 2 },
                        new WorkoutCardDTO { Name = "Workout C", DifficultyLevel = 7 },
                        new WorkoutCardDTO { Name = "Workout D", DifficultyLevel = 4 }
                    };
                string difficultyLevelGroup = "4-7";

                // Act
                var result = _workoutService.ApplyDifficultyLevelFilter(workoutsDTOs, difficultyLevelGroup);

                // Assert
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.IsTrue(result.All(w => w.DifficultyLevel >= 4 && w.DifficultyLevel <= 7));
            }

            [Test]
            public void ApplyDifficultyLevelFilter_ReturnsFilteredWorkouts_WhenDifficultyLevelGroupIs8to10()
            {
                // Arrange
                var workoutsDTOs = new List<WorkoutCardDTO>
                    {
                        new WorkoutCardDTO { Name = "Workout A", DifficultyLevel = 7 },
                        new WorkoutCardDTO { Name = "Workout B", DifficultyLevel = 9 },
                        new WorkoutCardDTO { Name = "Workout C", DifficultyLevel = 10 },
                        new WorkoutCardDTO { Name = "Workout D", DifficultyLevel = 8 }
                    };
                string difficultyLevelGroup = "8-10";

                // Act
                var result = _workoutService.ApplyDifficultyLevelFilter(workoutsDTOs, difficultyLevelGroup);

                // Assert
                Assert.That(result.Count, Is.EqualTo(3));
                Assert.IsTrue(result.All(w => w.DifficultyLevel >= 8 && w.DifficultyLevel <= 10));
            }
        }

    }
}