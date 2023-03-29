using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class SurnameValidation :ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            var surname = value.ToString().Trim();

            if (surname.Any(k => !Char.IsLetter(k)) && surname.Length >= 2)
            {
                return false;
            }
            else if (surname.Length <= 1)
            {
                ErrorMessage = "Soyad Alanı En Az 2 Harften Oluşmalıdır!!!";
                return false;
            }
            return true;
        }
    }
}
