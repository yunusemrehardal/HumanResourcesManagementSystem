using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class FoundationYearValidation:ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            DateTime foundationYear = (DateTime)value;

            if (foundationYear>DateTime.Now || foundationYear.Year<1800)
            {
                return false;
            }
            return true;           
        }
    }
}
