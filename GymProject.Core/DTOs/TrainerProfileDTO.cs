using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.DTOs
{
    public class TrainerProfileDTO
    {
        public int Id { get; set; }


        public string FullName { get; set; } = string.Empty;

        public int Age { get; set; }

        public string FavouriteExercise { get; set; } = string.Empty;

        public string Slogan { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string? Education { get; set; }

        private bool _isDeleted;
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }

        private DateTime _DeleteTime;
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }
    }
}
