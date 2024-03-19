using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class AtLeastOneCheckedAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult(ErrorMessage ?? "The collection is null.");
        }

        if (!(value is List<string> selected))
        {
            return new ValidationResult("The value must be a List<string>.");
        }

        if (!selected.Any())
        {
            return new ValidationResult(ErrorMessage ?? "At least one entity must be selected.");
        }

        return ValidationResult.Success!;
    }
}