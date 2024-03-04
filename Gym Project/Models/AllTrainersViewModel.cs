using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gym_Project.Models
{
    public class AllTrainersViewModel
    {

        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public int Age { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public string Slogan { get; set; } = string.Empty;

    }
}
