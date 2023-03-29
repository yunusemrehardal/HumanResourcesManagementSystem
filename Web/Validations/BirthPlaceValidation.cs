using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.Validations
{
    public class BirthPlaceValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string receivedValue;

            if (value == null)
            {
                return true;
            }

            receivedValue = value.ToString().Trim();

            if (receivedValue == null)
            {
                return true;
            }

            if (receivedValue.Any(k => !Char.IsLetter(k)))
            {
                ErrorMessage = "Doğum Yeri Alanı Sadece Harflerden Oluşabilir!!!";
                return false;
            }
            return true;
        }
    }
}
