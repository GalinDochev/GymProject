using Gym_Project.Models.CalculatorModels;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Project.Controllers
{
    public class CalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View(new BMRInputModel());
        }

        public IActionResult Calculate(BMRInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View((new BMRInputModel()));
            }
            double bmr = 10 * model.Weight + 6.25 * model.Height - 5 * model.Age;
            bmr += model.Gender == "male" ? 5 : -161;
            bmr *= 1.2;
            bmr += model.Walking * 60 * (0.03 * model.Weight * 1 / 0.45) / 7;
            bmr += model.Cardio * 60 * (0.07 * model.Weight * 1 / 0.45) / 7;
            bmr = Math.Floor(bmr);

            int targetGainWeight = (int)(Math.Round((bmr + 300) / 100) * 100);
            int targetMaintain = (int)(Math.Round(bmr / 100) * 100);
            int targetLoseWeight = (int)(Math.Round((bmr - 500) / 100) * 100);

            model.TargetGainWeight = targetGainWeight;
            model.TargetMaintain = targetMaintain;
            model.TargetLoseWeight = targetLoseWeight;

            return View(nameof(Index),model); 
        }
    }
}
