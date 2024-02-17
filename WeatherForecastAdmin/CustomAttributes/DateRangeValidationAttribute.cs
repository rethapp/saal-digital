using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherForecastAdmin.CustomAttributes
{
    public class DateRangeValidationAttribute : ValidationAttribute
    {

        public DateRangeValidationAttribute() { }

        public string GetErrorMessage() => "Date must be before tomorrow";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (DateTime.Compare(date, DateTime.Now) > 0) return new ValidationResult(GetErrorMessage());
            else return ValidationResult.Success;
        }

    }
}
