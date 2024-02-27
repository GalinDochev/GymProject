using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymProject.Infrastructure.Migrations
{
    public partial class SeedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkoutCategories",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                comment: "Category Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComment: "Category Name");

            migrationBuilder.InsertData(
                table: "WorkoutCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cardio" },
                    { 2, "Strength Training" },
                    { 3, "Flexibility and Mobility" },
                    { 4, "Balance and Stability" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkoutCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkoutCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkoutCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WorkoutCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkoutCategories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                comment: "Category Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldComment: "Category Name");
        }
    }
}
