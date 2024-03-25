using System.ComponentModel.DataAnnotations;

namespace Gym_Project.Models.HomeModels
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "Please enter the recipient's email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string From { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the subject.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the message.")]
        public string Message { get; set; } = string.Empty;
    }
}
