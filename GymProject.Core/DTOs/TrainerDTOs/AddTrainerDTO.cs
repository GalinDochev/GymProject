using GymProject.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using GymProject.Common.Constants.TrainerDataConstants;

namespace GymProject.Core.DTOs.TrainerDTOs
{
    public class AddTrainerDTO
    {
        [Key]
        [Comment("Trainer identifier")]
        public int Id { get; set; }


        [Required]
        [Comment("Trainer's Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public int Age { get; set; }

        [Required]
        public int ExerciseId { get; set; }

        [Required]
        public List<ExerciseForTrainerDTO> Exercises { get; set; } = new List<ExerciseForTrainerDTO>();

        [Required]
        [Comment("The motto or slogan of the trainer")]
        public string Slogan { get; set; } = string.Empty;

        [Required]
        [Comment("A picture of the Trainer")]
        public string ImageUrl { get; set; } = string.Empty;

        [Comment("Trainers Education if he has one")]
        public string? Education { get; set; }

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }
    }
}
