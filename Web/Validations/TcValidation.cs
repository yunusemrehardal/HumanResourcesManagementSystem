using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class TcValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;
            string identificationNumber = value.ToString().Trim();

            int total = 0, sumOfEvenNumber = 0, sumOfOddNumber = 0;
            if (identificationNumber.Length != 11)
                return false;
            if (identificationNumber.Any(k => !Char.IsDigit(k)))
                return false;
            if (Convert.ToInt32(identificationNumber[0].ToString()) == 0)
                return false;
            if(identificationNumber=="11111111110")
                return false;

            for (int i = 0; i <= 9; i++)
            {
                if (i % 2 != 0 && i <= 7)
                    sumOfEvenNumber += Convert.ToInt32(identificationNumber[i].ToString());

                else if (i % 2 == 0 && i <= 8)
                    sumOfOddNumber += Convert.ToInt32(identificationNumber[i].ToString()); total += Convert.ToInt32(identificationNumber[i].ToString());
            }

            if ((7 * sumOfOddNumber - sumOfEvenNumber) % 10 != Convert.ToInt32(identificationNumber[9].ToString()))
                return false;
            if (total % 10 != Convert.ToInt32(identificationNumber[10].ToString()))
                return false;
            return true;
        }
    }
}
