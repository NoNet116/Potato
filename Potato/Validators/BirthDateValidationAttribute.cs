using System.ComponentModel.DataAnnotations;

namespace Potato.Validators
{
    public class BirthDateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var birthDate = (DateTime)value;
            var today = DateTime.Today;

            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;

            return age >= 18 && age <= 100;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Поле \"{name}\" должно содержать дату от 18 до 100 лет.";
        }
    }
}
