using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherForecastAdmin.CustomAttributes
{

    //EXPL: using standard validation attributes it was not possible to check if a date is not after than another date
    //so I created this special validator that extends the ValidationAttribute. This validation is used inside the Razor page
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
