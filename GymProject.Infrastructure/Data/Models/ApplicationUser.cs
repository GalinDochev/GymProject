using GymProject.Common.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GymProject.Infrastructure.Data.Models
{
    public class ApplicationUser:IdentityUser
    {

        [Required]
        [MaxLength(ApplicationUserDataConstants.MaxFirstNameLength)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(ApplicationUserDataConstants.MaxLastNameLength)]

        public string LastName { get; set; }= string.Empty;

        [Required]
        [Range(ApplicationUserDataConstants.MinAge,ApplicationUserDataConstants.MaxAge)]
        public int Age { get; set; } = 18;

        public ICollection<UserWorkout> UserWorkouts { get; set; } = new List<UserWorkout>();
    }
}
