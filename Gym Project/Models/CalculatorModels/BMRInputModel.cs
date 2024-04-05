using GymProject.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Gym_Project.Models.CalculatorModels
{
    public class BMRInputModel
    {
        [Required(ErrorMessage = "Age is required")]
        [Range(CalculatorDataConstants.MinAge, CalculatorDataConstants.MaxAge, ErrorMessage = "Age must be between {1} and {2}")]
        public int Age { get; set; } = CalculatorDataConstants.DefaultAge;

        [Required(ErrorMessage = "Height is required")]
        [Range(CalculatorDataConstants.MinHeight, CalculatorDataConstants.MaxHeight, ErrorMessage = "Height must be between {1} and {2}")]
        public int Height { get; set; } = CalculatorDataConstants.DefaultHeight;

        [Required(ErrorMessage = "Weight is required")]
        [Range(CalculatorDataConstants.MinWeight, CalculatorDataConstants.MaxWeight, ErrorMessage = "Weight must be between {1} and {2}")]
        public int Weight { get; set; } = CalculatorDataConstants.DefaultWeight;

        [Required(ErrorMessage = "Walking value is required")]
        [Range(CalculatorDataConstants.MinWalking, CalculatorDataConstants.MaxWalking, ErrorMessage = "Walking must be between {1} and {2}")]
        public int Walking { get; set; } = CalculatorDataConstants.DefaultWalking;

        [Required(ErrorMessage = "Cardio value is required")]
        [Range(CalculatorDataConstants.MinCardio, CalculatorDataConstants.MaxCardio, ErrorMessage = "Cardio must be between {1} and {2}")]
        public int Cardio { get; set; } = CalculatorDataConstants.DefaultCardio;
        [Required]
        public string Gender { get; set; } = string.Empty;

        public int TargetGainWeight { get; set; }
        public int TargetMaintain { get; set; }
        public int TargetLoseWeight { get; set; }
    }
}
