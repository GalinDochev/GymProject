using Gym_Project.Models.CalculatorModels;
using GymProject.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Project.Controllers
{
    public class CalculatorController : Controller
    {
        private ILogger<CalculatorController> _logger;
        public CalculatorController(ILogger<CalculatorController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                return View(new BMRInputModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the Calculate Index View.");
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult Calculate()
        {
            try
            {
                return View("Index", new BMRInputModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the Calculate Index View.");
                return BadRequest();
            }
        }

        public IActionResult Calculate(BMRInputModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ShowAlert = true;
                    return View((new BMRInputModel()));
                }
                double bmr =
                    CalculatorDataConstants.WeightMultiplier * model.Weight +
                    CalculatorDataConstants.HeightMultiplier * model.Height -
                    CalculatorDataConstants.AgeMultiplier * model.Age;

                bmr += model.Gender ==
                    CalculatorDataConstants.MaleGender
                    ? CalculatorDataConstants.MaleGenderAdjustment : -
                    CalculatorDataConstants.FemaleGenderAdjustment;

                bmr *= CalculatorDataConstants.ActivityLevelMultiplier;
                model.Cardio = 0;

                bmr += model.Walking * CalculatorDataConstants.MinutesPerWeek
                    * (CalculatorDataConstants.WalkingCoefficient *
                    model.Weight * CalculatorDataConstants.KilogramsToPounds)
                    / CalculatorDataConstants.DaysInWeek;

                bmr += model.Cardio * CalculatorDataConstants.MinutesPerWeek
                    * (CalculatorDataConstants.CardioCoefficient * model.Weight * CalculatorDataConstants.KilogramsToPounds) / CalculatorDataConstants.DaysInWeek;

                bmr = Math.Floor(bmr);

                int targetGainWeight = 
                    (int)(Math.Round((bmr + CalculatorDataConstants.WeightGainCalories) 
                    / CalculatorDataConstants.CaloriesPerPound)
                    * CalculatorDataConstants.CaloriesPerPound);

                int targetMaintain = (int)(Math.Round(bmr / CalculatorDataConstants.CaloriesPerPound) * CalculatorDataConstants.CaloriesPerPound);

                int targetLoseWeight = (int)(Math.Round((bmr - CalculatorDataConstants.WeightLossCalories) 
                    / CalculatorDataConstants.CaloriesPerPound) 
                    * CalculatorDataConstants.CaloriesPerPound);

                model.TargetGainWeight = targetGainWeight;
                model.TargetMaintain = targetMaintain;
                model.TargetLoseWeight = targetLoseWeight;
                return View(nameof(Index), model);
            }
            catch (InvalidOperationException ex)
            {
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = CustomExceptionMessages.CalculatorExceptionMessage;
                _logger.LogError(ex, "InvalidOperationException occurred in the Calculate action in the Calculator Controller.");
                return RedirectToAction(nameof(Index));
            }
            catch (DivideByZeroException ex)
            {
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = CustomExceptionMessages.CalculatorExceptionMessage;
                _logger.LogError(ex, "DivideByZeroException occurred in the Calculate action in the Calculator Controller.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ShowException"] = true;
                TempData["ExceptionMessage"] = CustomExceptionMessages.CalculatorExceptionMessage;
                _logger.LogError(ex, "An error  in the Calculate action in the Calculator Controller.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
