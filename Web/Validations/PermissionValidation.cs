using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Web.Validations
{
    public class PermissionValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("İşe Başlama Alanı Boş Olmaz!!!");
            }
            DateTime startDateOfPermission = (DateTime)value;

            if (startDateOfPermission < DateTime.Now.Date)
            {
                return new ValidationResult("Geçmiş tarihli bir izin talep edemezsiniz.");
            }
            else if (startDateOfPermission <= DateTime.Now.AddDays(1) || startDateOfPermission >= DateTime.Now.AddDays(90))
            {
                return new ValidationResult("İzin Başlangıç Tarihi En Az 1 , En Fazla 90 Gün Önce Belirlenmelidir.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }

    }
}
