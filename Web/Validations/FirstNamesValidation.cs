using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class FirstNamesValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            var name = value.ToString().Trim();

            if (name.Any(k => !Char.IsLetter(k)) && name.Length >= 3)
            {
                return false;
            }
            else if(name.Length <= 2 )
            {
                ErrorMessage = "İsim Alanı En Az 3 Harften Oluşmalıdır!!!";
                return false;
            }
            return true;
        }
    }
}
