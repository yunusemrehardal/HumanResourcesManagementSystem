using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class ContractStartYearValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            DateTime contractStartYear = (DateTime)value;

            if (contractStartYear <= new DateTime(1950, 1, 1))
            {
                ErrorMessage = "Sözleşme Başlama Tarihi Yakın bir tarih seçmelisiniz.!!!";
                return false;
            }

            if (contractStartYear > DateTime.Now)
            {
                ErrorMessage = "Sözleşme Başlama Tarihi Şimdiki Zamandan İleri Bir Tarih Olamaz.!!!";
                return false;
            }
            return true;
        }
    }
}
