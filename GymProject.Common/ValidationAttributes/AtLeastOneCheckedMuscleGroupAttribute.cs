using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class AtLeastOneCheckedMuscleGroupAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult(ErrorMessage ?? "The muscle groups collection is null.");
        }

        if (!(value is List<string> selectedMuscleGroups))
        {
            return new ValidationResult("The value must be a List<string>.");
        }

        if (!selectedMuscleGroups.Any())
        {
            return new ValidationResult(ErrorMessage ?? "At least one muscle group must be selected.");
        }

        return ValidationResult.Success!;
    }
}