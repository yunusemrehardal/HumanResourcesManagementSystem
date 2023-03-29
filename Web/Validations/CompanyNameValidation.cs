using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class CompanyNameValidation :  ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            var name = value.ToString().Trim();

            if (name.Any(k => !Char.IsLetterOrDigit(k)))
            {
                return false;
            }
            return true;
        }
    }
}
