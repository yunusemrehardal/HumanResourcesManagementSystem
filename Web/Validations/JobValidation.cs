using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class JobValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            var name = value.ToString().Trim();

            if (name.Any(k => k != ' ' && !Char.IsLetter(k)))
            {
                return false;
            }
            return true;
        }
    }
}
