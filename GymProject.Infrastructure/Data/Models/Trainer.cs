﻿using GymProject.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GymProject.Infrastructure.Constants.TrainerDataConstants;

namespace GymProject.Infrastructure.Data.Models
{
    public class Trainer : IDeletable
    {
        [Key]
        [Comment("Trainer identifier")]
        public int Id { get; set; }


        [Required]
        [MaxLength(MaxTrainerNameLength)]
        [Comment("Trainer's Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Range(18, 55)]
        public int Age { get; set; }

        [Required]
        [Comment("Trainer's Favourite Excercise identifier")]
        public int ExerciseId { get; set; }

        [Required]
        [Comment("The motto or slogan of the trainer")]
        [MaxLength(MaxTrainerSloganLength)]
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
