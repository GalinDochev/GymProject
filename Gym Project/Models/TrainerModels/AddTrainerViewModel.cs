﻿using GymProject.Common.Constants.ExcerciseDataConstants;
using GymProject.Common.Constants.TrainerDataConstants;
using GymProject.Core.DTOs.TrainerDTOs;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gym_Project.Models.TrainerModels
{
    public class AddTrainerViewModel
    {
        [Comment("Trainer identifier")]
        public int Id { get; set; }


        [Required]
        [StringLength(TrainerDataConstants.MaxTrainerNameLength, MinimumLength = TrainerDataConstants.MinTrainerNameLength)]
        [Comment("Trainer's Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Range(TrainerDataConstants.MinTrainerAge, TrainerDataConstants.MaxTrainerAge)]
        public int Age { get; set; }

        [Required]
        public int ExerciseId { get; set; }

        [Required]
        public List<ExerciseForTrainerDTO> Exercises { get; set; } = new List<ExerciseForTrainerDTO>();

        [Required]
        [Comment("The motto or slogan of the trainer")]
        [StringLength(TrainerDataConstants.MaxTrainerSloganLength, MinimumLength = TrainerDataConstants.MinTrainerSloganLength)]
        public string Slogan { get; set; } = string.Empty;

        [Required]
        [Comment("A picture of the Trainer")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Comment("Trainers Education if he has one")]
        public string? Education { get; set; }

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }
    }
}
