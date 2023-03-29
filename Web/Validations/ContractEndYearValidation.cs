using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Web.Validations
{
    public class ContractEndYearValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var vm = (CompanyViewModel)validationContext.ObjectInstance;

            if (value == null)
            {
                return ValidationResult.Success;
            }

            DateTime contractEndYear = (DateTime)value;

            if (vm.ContractStartYear > contractEndYear)
            {
                return new ValidationResult("Sözleşme Bitiş Tarihi Sözleşme Başlangıç Tarihinden Geri Bir Tarih Girilemez.!!!");
            }

            DateTime tenYearsLater = DateTime.Now.AddYears(10);

            if (contractEndYear > tenYearsLater)
            {
               return new ValidationResult ( "Sözleşme Bitiş Tarihi Geri Bir Tarih Girilemez.!!!");
              
            }

            if (contractEndYear <= DateTime.Now)
            {
                return new ValidationResult("Sözleşme Bitiş Tarihi Geri Bir Tarih Girilemez.!!!");
            }
            return ValidationResult.Success;
        }
    }
}
