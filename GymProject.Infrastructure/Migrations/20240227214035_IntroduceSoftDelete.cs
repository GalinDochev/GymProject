using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymProject.Infrastructure.Migrations
{
    public partial class IntroduceSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseMuscleGroup_Excercises_ExerciseId",
                table: "ExerciseMuscleGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseWorkouts_Excercises_ExerciseId",
                table: "ExerciseWorkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Excercises_ExerciseId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_ExerciseId",
                table: "Trainers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Workouts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Workouts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "WorkoutCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WorkoutCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "UsersWorkouts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UsersWorkouts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Trainers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Trainers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "MuscleGroups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MuscleGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "ExerciseWorkouts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExerciseWorkouts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "ExerciseMuscleGroup",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExerciseMuscleGroup",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Excercises",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Excercises",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseMuscleGroup_Excercises_ExerciseId",
                table: "ExerciseMuscleGroup",
                column: "ExerciseId",
                principalTable: "Excercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseWorkouts_Excercises_ExerciseId",
                table: "ExerciseWorkouts",
                column: "ExerciseId",
                principalTable: "Excercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseMuscleGroup_Excercises_ExerciseId",
                table: "ExerciseMuscleGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseWorkouts_Excercises_ExerciseId",
                table: "ExerciseWorkouts");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "WorkoutCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WorkoutCategories");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "UsersWorkouts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UsersWorkouts");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "MuscleGroups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MuscleGroups");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "ExerciseWorkouts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExerciseWorkouts");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "ExerciseMuscleGroup");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExerciseMuscleGroup");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Excercises");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Excercises");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_ExerciseId",
                table: "Trainers",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseMuscleGroup_Excercises_ExerciseId",
                table: "ExerciseMuscleGroup",
                column: "ExerciseId",
                principalTable: "Excercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseWorkouts_Excercises_ExerciseId",
                table: "ExerciseWorkouts",
                column: "ExerciseId",
                principalTable: "Excercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Excercises_ExerciseId",
                table: "Trainers",
                column: "ExerciseId",
                principalTable: "Excercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
