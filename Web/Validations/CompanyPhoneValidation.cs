using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Web.Validations
{
    public class CompanyPhoneValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            string companyPhoneNumber = value.ToString();

            if (!Regex.IsMatch(companyPhoneNumber, @"^\+90\d{10}$|^0\d{10}$"))
            {
                ErrorMessage = "Yazdığınız Numara Şirket Telefon Numarası İçin Yanlış Formattadır.";
                return false;
            }
            return true;
        }
    }
}
