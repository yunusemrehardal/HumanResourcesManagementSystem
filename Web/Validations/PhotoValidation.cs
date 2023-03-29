using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class PhotoValidation : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            int maxPhotoSize = 2048;
            IFormFile formFile = value as IFormFile;

            if (formFile == null)
                return true;

            if (formFile.Length > maxPhotoSize * 1024)
            {
                ErrorMessage = $"Fotograf boyutu çok büyük. Maksimum boyut : {maxPhotoSize}kb.!!!";
                return false;
            }

            if (formFile.ContentType.EndsWith("png") || formFile.ContentType.EndsWith("jpg") || formFile.ContentType.EndsWith("jpeg"))
            {
                return true;
            }
            else
            {
                ErrorMessage = $"Fotograf uzantısı sadece png ile jpg kabul edilir.!!!";
                return false;
            }
        }
    }
}
