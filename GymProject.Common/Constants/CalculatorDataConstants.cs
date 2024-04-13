using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Common.Constants
{
    public class CalculatorDataConstants
    {

        public const int DefaultAge =13;
        public const int DefaultHeight = 80;
        public const int DefaultWeight = 40;
        public const int DefaultWalking = 0;
        public const int DefaultCardio = 0;


        public const int MinAge = 13;
        public const int MaxAge = 100;
        public const int MinHeight = 80;
        public const int MaxHeight = 250;
        public const int MinWeight = 40;
        public const int MaxWeight = 200;
        public const int MinWalking = 0;
        public const int MaxWalking = 50;
        public const int MinCardio = 0;
        public const int MaxCardio = 50;

        public const double WeightMultiplier = 10;
        public const double HeightMultiplier = 6.25;
        public const double AgeMultiplier = 5;

        public const double MaleGenderAdjustment = 5;
        public const double FemaleGenderAdjustment = -161;
        public const string MaleGender = "male";
        public const string FemaleGender = "female";

        public const double ActivityLevelMultiplier = 1.2;

        public const double WalkingCoefficient = 0.03;
        public const double CardioCoefficient = 0.07;

        
        public const double MinutesPerWeek = 60;
        public const double KilogramsToPounds = 1 / 0.45;
        public const int CaloriesPerPound = 100;
        public const int WeightGainCalories = 300;
        public const int WeightLossCalories = 500;
        public const int DaysInWeek = 7;
    }
}
