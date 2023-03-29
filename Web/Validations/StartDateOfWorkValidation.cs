using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Web.Validations
{
    public class StartDateOfWorkValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null)
            {
                return new ValidationResult("İşe Başlama Alanı Boş Olmaz!!!");
            }

            DateTime startDateOfWork = (DateTime)value;

            if (startDateOfWork.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                return ValidationResult.Success;
            }

            if (startDateOfWork <= DateTime.Now || startDateOfWork >= DateTime.Now.AddDays(10))
            {
                return new ValidationResult("Başlama Tarihi 10 gün ileri olabilir.");
            }

            return ValidationResult.Success;
        }
    }
}