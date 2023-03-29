using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Web.Validations
{
    public class EndDateOfWorkValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            DateTime endDateOfWork = (DateTime)value;

            try
            {
                var vm = (PersonelViewModel)validationContext.ObjectInstance;

                if (vm.StartDateOfWork.Value > endDateOfWork)
                {
                    return new ValidationResult("İşten Çıkış Tarihi, İşten Giriş Tarihinden Geri Bir Tarih Girilemez.!!!");
                }

                if (endDateOfWork >= new DateTime(2050, 1, 1))
                {
                    return new ValidationResult("İşe Giriş Tarihi Yakın Bir Tarih Girilmelidir.!!!");
                }

                return ValidationResult.Success;
            }
            catch (Exception)
            {
                try
                {
                    var vm2 = (ManagerViewModel)validationContext.ObjectInstance;

                    if (vm2.StartDateOfWork.Value > endDateOfWork)
                    {
                        return new ValidationResult("İşten Çıkış Tarihi, İşten Giriş Tarihinden Geri Bir Tarih Girilemez.!!!");
                    }

                    if (endDateOfWork >= new DateTime(2050, 1, 1))
                    {
                        return new ValidationResult("İşe Giriş Tarihi Yakın Bir Tarih Girilmelidir.!!!");
                    }

                    return ValidationResult.Success;
                }
                catch (Exception)
                {
                    return new ValidationResult("Geçersiz alan.");
                }
            }
        }
    }
}
