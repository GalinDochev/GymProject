using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymProject.Infrastructure.Migrations
{
    public partial class SeedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersWorkouts",
                columns: table => new
                {
                    WorkoutId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersWorkouts", x => new { x.WorkoutId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersWorkouts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersWorkouts_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Excercises",
                columns: new[] { "Id", "Description", "DifficultyLevel", "ImageUrl", "Name", "Repetitions", "Sets" },
                values: new object[,]
                {
                    { 1, "Description of Dumbbell Biceps Curl", 6, "https://cdn-0.weighttraining.guide/wp-content/uploads/2016/05/Dumbbell-Alternate-Biceps-Curl-resized.png?ezimgfmt=ng%3Awebp%2Fngcb4", "Dumbbell Biceps Curl", 10, 4 },
                    { 2, "Description of Barbell Biceps Curl", 7, "https://kinxlearning.com/cdn/shop/files/exercise-41_1400x.jpg?v=1613157966", "Barbell Biceps curl", 12, 4 },
                    { 3, "Description of Triceps French Curl", 8, "https://www.fitstep.com/2/2-how-to-build-muscle/muscle-and-strength-questions/graphics/french-curl.gif", "Triceps French Curl", 8, 4 },
                    { 4, "Description of Cable Rope Pushdown", 7, "https://static.strengthlevel.com/images/illustrations/tricep-rope-pushdown-1000x1000.jpg", "Cable Rope Pushdown", 12, 4 }
                });

            migrationBuilder.InsertData(
                table: "MuscleGroups",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Quadriceps" },
                    { 2, "Hamstrings" },
                    { 3, "Calves" },
                    { 4, "Chest" },
                    { 5, "Back" },
                    { 6, "Biceps" },
                    { 7, "Triceps" },
                    { 8, "Shoulders" }
                });

            migrationBuilder.InsertData(
                table: "ExerciseMuscleGroup",
                columns: new[] { "ExerciseId", "MuscleGroupId" },
                values: new object[,]
                {
                    { 1, 6 },
                    { 2, 6 },
                    { 3, 7 },
                    { 4, 7 }
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "Age", "Education", "ExerciseId", "FullName", "ImageUrl", "Slogan" },
                values: new object[,]
                {
                    { 1, 30, "Certified Personal Trainer", 1, "Larry Wheels", "https://giants-live.com/app/uploads/2022/01/larry-wheels.jpg", "Train hard, win easy" },
                    { 2, 25, "Bachelor's Degree in Exercise Science", 2, "Jane Smith", "https://img.freepik.com/premium-photo/young-female-fitness-personal-trainer-with-notepad-standing-gym-with-thumb-up_146671-31568.jpg", "Fitness is a journey, not a destination" },
                    { 3, 27, "Certified Personal Trainer", 3, "John Doe", "https://t3.ftcdn.net/jpg/06/45/17/94/360_F_645179444_EtQDcQw5Mcyv1MSH25K5FrEkb3LfH5Vk.jpg", "Train hard, win easy" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersWorkouts_UserId",
                table: "UsersWorkouts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersWorkouts");

            migrationBuilder.DeleteData(
                table: "ExerciseMuscleGroup",
                keyColumns: new[] { "ExerciseId", "MuscleGroupId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "ExerciseMuscleGroup",
                keyColumns: new[] { "ExerciseId", "MuscleGroupId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "ExerciseMuscleGroup",
                keyColumns: new[] { "ExerciseId", "MuscleGroupId" },
                keyValues: new object[] { 3, 7 });

            migrationBuilder.DeleteData(
                table: "ExerciseMuscleGroup",
                keyColumns: new[] { "ExerciseId", "MuscleGroupId" },
                keyValues: new object[] { 4, 7 });

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Excercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Excercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Excercises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Excercises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
