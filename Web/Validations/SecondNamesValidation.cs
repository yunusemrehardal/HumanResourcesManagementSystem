using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class SecondNamesValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            var name = value.ToString().Trim();

            if (name == null)
            {
                return true;
            }
            else if (name != null)
            {
                if (name.Any(k => !Char.IsLetter(k)) && name.Length >= 3)
                {
                    return false;
                }
                else if (name.Length <= 2)
                {
                    ErrorMessage = "İsim Alanı En Az 3 Harften Oluşmalıdır!!!";
                    return false;
                }
            }
            return true;
        }
    }
}
