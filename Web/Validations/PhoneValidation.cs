using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Web.Validations
{
    public class PhoneValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            string phoneNumber = value.ToString();

            if ((!Regex.IsMatch(phoneNumber, @"^\+90\d{10}$|^0\d{10}$") || !phoneNumber.StartsWith("05")) && phoneNumber.Length ==11)
            {
                ErrorMessage = "Yazdığınız Numara Telefon Numarası İçin Yanlış Formattadır.";
                return false;
                
            }
            else if (phoneNumber.Length != 11)
            {
                ErrorMessage = "Telefon Numarası 11 Haneli Olmalıdır.";
                return false;
            }
            return true;
        }

    }
}
