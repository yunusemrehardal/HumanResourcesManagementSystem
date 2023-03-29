using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Web.Validations
{
    public class BirthDateValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            DateTime birthDate = (DateTime)value;
            int age = DateTime.Now.Year - birthDate.Year;

            if (age >= 18 && age <= 65)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
