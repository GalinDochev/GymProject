using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gym_Project.Models.TrainerModels
{
    public class AllTrainersViewModel
    {

        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public int Age { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string Slogan { get; set; } = string.Empty;

    }
}
