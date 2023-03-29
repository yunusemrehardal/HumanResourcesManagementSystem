using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class MaasValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
              
            if (value != null)
            {
                return  ValidationResult.Success;
            }
            if ((decimal)value == 0)
            {
                return new ValidationResult("Maaş Alanı Boş Olamaz");
            }
            if ((decimal)value < 8500m)
            {
                return new ValidationResult($"Maaş asgari ücretten {value} düşük olamaz.");
            }

            if ((decimal)value > 500000m)
            {
                return new ValidationResult($"Maaş {value} den daha fazla olamaz.");
            }

            return ValidationResult.Success;
        }
    }
}
