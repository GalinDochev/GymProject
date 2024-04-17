using GymProject.Core.DTOs.TrainerDTOs;
using GymProject.Core.Services;
using GymProject.Infrastructure.Data;
using GymProject.Infrastructure.Data.Models;
using GymProject.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
namespace GymProject.Services.Tests
{
    [TestFixture]
    public class TrainerServiceUnitTests
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext _dbContext;
        private Mock<TrainersRepository> _mockTrainerRepository;
        private Mock<ExerciseRepository> _mockExerciseRepository;
        private Mock<ILogger<TrainersService>> _mockLogger;
        private TrainersService _trainerService;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new ApplicationDbContext(_options);
            _mockTrainerRepository = new Mock<TrainersRepository>(_dbContext);
            _mockExerciseRepository = new Mock<ExerciseRepository>(_dbContext);
            _mockLogger = new Mock<ILogger<TrainersService>>();
            _trainerService = new TrainersService(_mockTrainerRepository.Object, _mockExerciseRepository.Object, _mockLogger.Object);
        }
        [Test]
        public async Task GetAllNotDeletedTrainers_ReturnsTrainerDTOList()
        {
            var trainers = new List<Trainer>
        {
            new Trainer { Id = 1, FullName = "John Doe", Age = 30, ImageUrl = "image.jpg", Slogan = "Train hard, achieve greatness" },
            new Trainer { Id = 2, FullName = "Jane Smith", Age = 25, ImageUrl = "image2.jpg", Slogan = "Fitness is life" }
        };
            _mockTrainerRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(trainers);

            var result = await _trainerService.GetAllNotDeletedTrainers();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FullName, Is.EqualTo("John Doe"));
            Assert.That(result[0].Age, Is.EqualTo(30));
            Assert.That(result[0].ImageUrl, Is.EqualTo("image.jpg"));
            Assert.That(result[0].Slogan, Is.EqualTo("Train hard, achieve greatness"));

            _mockTrainerRepository.Verify(repo => repo.GetAllNotDeleted(), Times.Once);
        }

        [Test]
        public async Task GetAllNotDeletedTrainers_ReturnsEmptyList_WhenNoTrainers()
        {
            _mockTrainerRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(new List<Trainer>());

            var result = await _trainerService.GetAllNotDeletedTrainers();

            Assert.IsEmpty(result);

            _mockTrainerRepository.Verify(repo => repo.GetAllNotDeleted(), Times.Once);
        }

        [Test]
        public async Task GetTrainerByIdForProfile_ReturnsTrainerProfileDTO()
        {
            int trainerId = 1;
            var expectedTrainer = new Trainer
            {
                Id = trainerId,
                FullName = "John Doe",
                Age = 30,
                Slogan = "Train hard, achieve greatness",
                Education = "Bachelor of Science in Physical Education",
                ExerciseId = 1,
                ImageUrl = "image.jpg",
                IsDeleted = false,
                DeleteTime = default(DateTime)
            };
            var expectedExercise = new Exercise { Id = 1, Name = "Running" };
            _mockTrainerRepository.Setup(repo => repo.GetById(trainerId)).ReturnsAsync(expectedTrainer);
            _mockExerciseRepository.Setup(repo => repo.GetById(expectedTrainer.ExerciseId)).ReturnsAsync(expectedExercise);

            var result = await _trainerService.GetTrainerByIdForProfile(trainerId);

            Assert.NotNull(result);
            Assert.That(result.FullName, Is.EqualTo(expectedTrainer.FullName));
            Assert.That(result.Age, Is.EqualTo(expectedTrainer.Age));
            Assert.That(result.Id, Is.EqualTo(expectedTrainer.Id));
            Assert.That(result.Slogan, Is.EqualTo(expectedTrainer.Slogan));
            Assert.That(result.Education, Is.EqualTo(expectedTrainer.Education));
            Assert.That(result.FavouriteExercise, Is.EqualTo(expectedExercise.Name));
            Assert.That(result.ImageUrl, Is.EqualTo(expectedTrainer.ImageUrl));
            Assert.That(result.IsDeleted, Is.EqualTo(expectedTrainer.IsDeleted));
            Assert.That(result.DeleteTime, Is.EqualTo(expectedTrainer.DeleteTime));

            _mockTrainerRepository.Verify(repo => repo.GetById(trainerId), Times.Once);
            _mockExerciseRepository.Verify(repo => repo.GetById(expectedTrainer.ExerciseId), Times.Once);
        }
        [Test]
        public void GetTrainerByIdForProfile_ThrowsException_WhenTrainerNotFound()
        {
            int trainerId = 1;
            _mockTrainerRepository.Setup(repo => repo.GetById(trainerId)).ReturnsAsync((Trainer)null);

            Assert.ThrowsAsync<NullReferenceException>(async () => await _trainerService.GetTrainerByIdForProfile(trainerId));

            _mockTrainerRepository.Verify(repo => repo.GetById(trainerId), Times.Once);
            _mockExerciseRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetTrainerByIdForProfile_ThrowsException_WhenExerciseNotFound()
        {
            int trainerId = 1;
            var expectedTrainer = new Trainer { Id = trainerId, ExerciseId = 1 };
            _mockTrainerRepository.Setup(repo => repo.GetById(trainerId)).ReturnsAsync(expectedTrainer);
            _mockExerciseRepository.Setup(repo => repo.GetById(expectedTrainer.ExerciseId)).ReturnsAsync((Exercise)null);

            Assert.ThrowsAsync<NullReferenceException>(async () => await _trainerService.GetTrainerByIdForProfile(trainerId));

            _mockTrainerRepository.Verify(repo => repo.GetById(trainerId), Times.Once);
            _mockExerciseRepository.Verify(repo => repo.GetById(expectedTrainer.ExerciseId), Times.Once);
        }

        [Test]
        public async Task GetTrainerDTOModel_ReturnsValidModel()
        {
            var expectedExercises = new List<Exercise>
                {
                    new Exercise { Id = 1, Name = "Exercise 1" },
                    new Exercise { Id = 2, Name = "Exercise 2" },
                };

            _mockExerciseRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(expectedExercises);

            var result = await _trainerService.GetTrainerDTOModel();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Exercises);
            Assert.That(result.Exercises.Count, Is.EqualTo(expectedExercises.Count));

            for (int i = 0; i < expectedExercises.Count; i++)
            {
                Assert.That(result.Exercises[i].Id, Is.EqualTo(expectedExercises[i].Id));
                Assert.That(result.Exercises[i].Name, Is.EqualTo(expectedExercises[i].Name));
            }

            _mockExerciseRepository.Verify(repo => repo.GetAllNotDeleted(), Times.Once);
        }
        [Test]
        public async Task AddTrainer_SuccessfullyAddsTrainer()
        {
            var trainerToAdd = new AddTrainerDTO
            {
                Id = 1,
                Age = 30,
                Education = "PhD",
                ExerciseId = 1,
                FullName = "John Doe",
                ImageUrl = "image.jpg",
                Slogan = "Train hard, live strong"
            };

            _mockTrainerRepository.Setup(repo => repo.Add(It.IsAny<Trainer>())).Returns(Task.CompletedTask);

            await _trainerService.AddTrainer(trainerToAdd);

            _mockTrainerRepository.Verify(repo => repo.Add(It.IsAny<Trainer>()), Times.Once);
        }
        [Test]
        public async Task AddTrainer_ThrowsDbUpdateException_WhenDatabaseUpdateFails()
        {
            var trainerToAdd = new AddTrainerDTO
            {
                Id = 1,
                Age = 35,
                FullName = "Alice Smith",
                Education = "Certified Personal Trainer",
                ExerciseId = 2, 
                ImageUrl = "avatar.jpg",
                Slogan = "Fitness is a lifestyle!"
            };

            _mockTrainerRepository.Setup(repo => repo.Add(It.IsAny<Trainer>())).ThrowsAsync(new DbUpdateException());

            // Act & Assert
             Assert.ThrowsAsync<DbUpdateException>(async () => await _trainerService.AddTrainer(trainerToAdd));
        }

        [Test]
        public async Task GetTrainerByIdForEdit_ReturnsTrainerDTO()
        {
            // Arrange
            int trainerId = 1;
            var trainer = new Trainer
            {
                Id = trainerId,
                Age = 30,
                FullName = "John Doe",
                Education = "Certified Trainer",
                ExerciseId = 1,
                ImageUrl = "john.jpg",
                Slogan = "Train hard, achieve greatness!"
            };

            var exercises = new List<Exercise>
                    {
                        new Exercise { Id = 1, Name = "Exercise 1" },
                        new Exercise { Id = 2, Name = "Exercise 2" }
                    };

            _mockTrainerRepository.Setup(repo => repo.GetById(trainerId)).ReturnsAsync(trainer);
            _mockExerciseRepository.Setup(repo => repo.GetAllNotDeleted()).ReturnsAsync(exercises);

            // Act
            var result = await _trainerService.GetTrainerByIdForEdit(trainerId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(trainerId));
            Assert.That(result.Age, Is.EqualTo(trainer.Age));
            Assert.That(result.FullName, Is.EqualTo(trainer.FullName));
            Assert.That(result.Education, Is.EqualTo(trainer.Education));
            Assert.That(result.ExerciseId, Is.EqualTo(trainer.ExerciseId));
            Assert.That(result.ImageUrl, Is.EqualTo(trainer.ImageUrl));
            Assert.That(result.Slogan, Is.EqualTo(trainer.Slogan));
            Assert.That(result.Exercises.Count, Is.EqualTo(exercises.Count));
            Assert.That(result.Exercises[0].Name, Is.EqualTo(exercises[0].Name));
            Assert.That(result.Exercises[1].Name, Is.EqualTo(exercises[1].Name));
        }

        [Test]
        public async Task EditTrainer_UpdatesTrainer()
        {
            var trainerDTO = new AddTrainerDTO
            {
                Id = 1,
                FullName = "John Doe",
                Age = 30,
                ExerciseId = 1,
                Education = "Certified Trainer",
                Slogan = "Train hard, achieve greatness!",
                ImageUrl = "john.jpg"
            };

            var trainerToEdit = new Trainer
            {
                Id = trainerDTO.Id,
                FullName = "Old Name",
                Age = 25,
                ExerciseId = 2,
                Education = "Old Education",
                Slogan = "Old Slogan",
                ImageUrl = "old.jpg"
            };

            _mockTrainerRepository.Setup(repo => repo.GetById(trainerDTO.Id)).ReturnsAsync(trainerToEdit);

            await _trainerService.EditTrainer(trainerDTO);

            _mockTrainerRepository.Verify(repo => repo.Update(It.IsAny<Trainer>()), Times.Once);
            Assert.That(trainerToEdit.FullName, Is.EqualTo(trainerDTO.FullName));
            Assert.That(trainerToEdit.Age, Is.EqualTo(trainerDTO.Age));
            Assert.That(trainerToEdit.ExerciseId, Is.EqualTo(trainerDTO.ExerciseId));
            Assert.That(trainerToEdit.Education, Is.EqualTo(trainerDTO.Education));
            Assert.That(trainerToEdit.Slogan, Is.EqualTo(trainerDTO.Slogan));
            Assert.That(trainerToEdit.ImageUrl, Is.EqualTo(trainerDTO.ImageUrl));
        }

        [Test]
        public async Task DeleteTrainer_DeletesTrainer()
        {
            int trainerId = 1;
            var trainerToDelete = new Trainer { Id = trainerId };

            _mockTrainerRepository.Setup(repo => repo.GetById(trainerId)).ReturnsAsync(trainerToDelete);

            await _trainerService.DeleteTrainer(trainerId);

            _mockTrainerRepository.Verify(repo => repo.Delete(trainerToDelete), Times.Once);
        }
        [Test]
        public async Task DeleteTrainer_ThrowsException_WhenTrainerNotFound()
        {
            int trainerId = 1;

            _mockTrainerRepository.Setup(repo => repo.GetById(trainerId)).ThrowsAsync(new InvalidOperationException());

            var exception =  Assert.ThrowsAsync<InvalidOperationException>(() => _trainerService.DeleteTrainer(trainerId));
            Assert.NotNull(exception);
        }

    }

}
