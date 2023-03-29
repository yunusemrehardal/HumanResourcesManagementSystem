using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class EMailValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string receivedValue;

            if (value == null)
            {
                return false;
            }

            receivedValue = value.ToString().Trim();

            if (receivedValue.Split("@").Length != 2 || receivedValue.Contains(" "))
            {
                return false;
            }

            if (receivedValue.EndsWith("@bilgeadamboost.com"))
            {   
                return true;
            }
            return false;
        }
    }
}
