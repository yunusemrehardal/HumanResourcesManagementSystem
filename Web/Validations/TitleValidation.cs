using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class TitleValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null)
            {
                return new ValidationResult("Ünvansız şirket olamaz");
            }
            if (string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult("Ünvansız şirket olamaz");

            // Anonim Şirket (A.Ş.)
            if (value.ToString().EndsWith("A.Ş."))
                return ValidationResult.Success;

            // Limited Şirket (Ltd. Şti.)
            if (value.ToString().EndsWith("Ltd. Şti."))
                return ValidationResult.Success;

            // Komandit Şirket (K.Ş.)
            if (value.ToString().EndsWith("K.Ş."))
                return ValidationResult.Success;

            // Serbest İşletme (S.İ.)
            if (value.ToString().EndsWith("S.İ."))
                return ValidationResult.Success;

            // Kooperatif (Koop.)
            if (value.ToString().EndsWith("Koop."))
                return ValidationResult.Success;

            // Dernek
            if (value.ToString().EndsWith("Derneği"))
                return ValidationResult.Success;

            // Vakıf
            if (value.ToString().EndsWith("Vakfı"))
                return ValidationResult.Success;

          return new ValidationResult("Hatalı giriş yaptınız.( Ünvanlar : A.Ş. , Ltd. Şti. , K.Ş. , S.İ., Koop. , Derneği , Vakfı )");


        }
    }
}
