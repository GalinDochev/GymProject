using GymProject.Infrastructure.Data.Models;

namespace GymProject.Infrastructure.Data.SeedDatabase
{
    internal class SeedData
    {
        public Exercise FirstExercise { get; set; }
        public Exercise SecondExercise { get; set; }
        public Exercise ThirdExercise { get; set; }
        public Exercise FourthExercise { get; set; }
        public Category FirstCategory { get; set; }
        public Category SecondCategory { get; set; }
        public Category ThirdCategory { get; set; }
        public Category FourthCategory { get; set; }
        public Trainer FirstTrainer { get; set; }
        public Trainer SecondTrainer { get; set; }
        public Trainer ThirdTrainer { get; set; }
        public MuscleGroup FirstMuscleGroup { get; set; }
        public MuscleGroup SecondMuscleGroup { get; set; }
        public MuscleGroup ThirdMuscleGroup { get; set; }
        public MuscleGroup FourthMuscleGroup { get; set; }
        public MuscleGroup FifthMuscleGroup { get; set; }
        public MuscleGroup SixthMuscleGroup { get; set; }
        public MuscleGroup SeventhMuscleGroup { get; set; }
        public MuscleGroup EightMuscleGroup { get; set; }
        public ExerciseMuscleGroup FirstExerciseMuscleGroup { get; set; }
        public ExerciseMuscleGroup SecondExerciseMuscleGroup { get; set; }
        public ExerciseMuscleGroup ThirdExerciseMuscleGroup { get; set; }
        public ExerciseMuscleGroup FourthExerciseMuscleGroup { get; set; }

        public SeedData()
        {
            SeedExercises();
            SeedCategories();
            SeedTrainers();
            SeedMuscleGroups();
            SeedExerciseMuscleGroups();

        }
        private void SeedExercises()
        {
            FirstExercise =
                new Exercise
                {
                    Id = 1,
                    Name = "Dumbbell Biceps Curl",
                    Description = "Description of Dumbbell Biceps Curl",
                    DifficultyLevel = 6,
                    Repetitions = 10,
                    ImageUrl = "https://cdn-0.weighttraining.guide/wp-content/uploads/2016/05/Dumbbell-Alternate-Biceps-Curl-resized.png?ezimgfmt=ng%3Awebp%2Fngcb4",
                    Sets = 4
                };
            SecondExercise =
                new Exercise
                {
                    Id = 2,
                    Name = "Barbell Biceps curl",
                    Description = "Description of Barbell Biceps Curl",
                    DifficultyLevel = 7,
                    Repetitions = 12,
                    ImageUrl = "https://kinxlearning.com/cdn/shop/files/exercise-41_1400x.jpg?v=1613157966",
                    Sets = 4

                };
            ThirdExercise =
            new Exercise
            {
                Id = 3,
                Name = "Triceps French Curl",
                Description = "Description of Triceps French Curl",
                DifficultyLevel = 8,
                Repetitions = 8,
                ImageUrl = "https://www.fitstep.com/2/2-how-to-build-muscle/muscle-and-strength-questions/graphics/french-curl.gif",
                Sets = 4
            };
            FourthExercise =
                  new Exercise
                  {
                      Id = 4,
                      Name = "Cable Rope Pushdown",
                      Description = "Description of Cable Rope Pushdown",
                      DifficultyLevel = 7,
                      Repetitions = 12,
                      ImageUrl = "https://static.strengthlevel.com/images/illustrations/tricep-rope-pushdown-1000x1000.jpg",
                      Sets = 4
                  };
        }

        private void SeedCategories()
        {
            FirstCategory =
               new Category
               {
                   Id = 1,
                   Name = "Cardio"
               };
            SecondCategory =
                new Category
                {
                    Id = 2,
                    Name = "Strength Training"
                };
            ThirdCategory =
            new Category
            {
                Id = 3,
                Name = "Flexibility and Mobility"
            };
            FourthCategory =

           new Category
           {
               Id = 4,
               Name = "Balance and Stability"
           };
        }

        private void SeedTrainers()
        {
            FirstTrainer =
            new Trainer
            {
                Id = 1,
                FullName = "Larry Wheels",
                Age = 30,
                ExerciseId = 1,
                Slogan = "Train hard, win easy",
                ImageUrl = "https://giants-live.com/app/uploads/2022/01/larry-wheels.jpg",
                Education = "Certified Personal Trainer"
            };

            SecondTrainer =
            new Trainer
            {
                Id = 2,
                FullName = "Jane Smith",
                Age = 25,
                ExerciseId = 2,
                Slogan = "Fitness is a journey, not a destination",
                ImageUrl = "https://img.freepik.com/premium-photo/young-female-fitness-personal-trainer-with-notepad-standing-gym-with-thumb-up_146671-31568.jpg",
                Education = "Bachelor's Degree in Exercise Science"
            };
            ThirdTrainer =
            new Trainer
            {
                Id = 3,
                FullName = "John Doe",
                Age = 27,
                ExerciseId = 3,
                Slogan = "Train hard, win easy",
                ImageUrl = "https://t3.ftcdn.net/jpg/06/45/17/94/360_F_645179444_EtQDcQw5Mcyv1MSH25K5FrEkb3LfH5Vk.jpg",
                Education = "Certified Personal Trainer"
            };
        }

        public void SeedMuscleGroups()
        {
            FirstMuscleGroup =
                 new MuscleGroup
                 {
                     Id = 1,
                     Name = "Quadriceps"
                 };
            SecondMuscleGroup =
                new MuscleGroup
                {
                    Id = 2,
                    Name = "Hamstrings"
                };
                ThirdMuscleGroup =
                new MuscleGroup
                {
                    Id = 3,
                    Name = "Calves"
                };
            FourthMuscleGroup =
                new MuscleGroup
                {
                    Id = 4,
                    Name = "Chest"
                };
            FifthMuscleGroup =
                new MuscleGroup
                {
                    Id = 5,
                    Name = "Back"
                };
            SixthMuscleGroup =
                new MuscleGroup
                {
                    Id = 6,
                    Name = "Biceps"
                };
            SeventhMuscleGroup =
                new MuscleGroup
                {
                    Id = 7,
                    Name = "Triceps"
                };
            EightMuscleGroup =
                new MuscleGroup
                {
                    Id = 8,
                    Name = "Shoulders"
                };
        }

        public void SeedExerciseMuscleGroups()
        {
            FirstExerciseMuscleGroup =
            new ExerciseMuscleGroup
            {
                ExerciseId = 1,
                MuscleGroupId = 6
            };
            SecondExerciseMuscleGroup =
            new ExerciseMuscleGroup
            {
                ExerciseId = 2,
                MuscleGroupId = 6
            };
            ThirdExerciseMuscleGroup =
            new ExerciseMuscleGroup
            {
                ExerciseId = 3,
                MuscleGroupId = 7
            };
            FourthExerciseMuscleGroup =
            new ExerciseMuscleGroup
            {
                ExerciseId = 4,
                MuscleGroupId = 7
            };
        }
    }
}
