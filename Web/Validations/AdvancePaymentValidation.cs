using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Web.Models;

namespace Web.Validations
{
    public class AdvancePaymentValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var vm = (AdvanceViewModel)validationContext.ObjectInstance;
            if (vm != null)
            {
                var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
                var user = httpContextAccessor.HttpContext.User;

                if (user.IsInRole("Personel"))
                {
                    var _db = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
                    var _userManager = (UserManager<ApplicationUser>)validationContext.GetService(typeof(UserManager<ApplicationUser>));
                    var personelName = user.Identity.Name;
                    var activeUser = _userManager.Users.FirstOrDefault(u => u.UserName == personelName);
                    var activePersonel = _db.Personels.FirstOrDefault(x => x.AppUserId == activeUser.Id);
                    var advanceRequests = _db.Advances.Where(x => x.PersonelId == activePersonel.Id);


                    var sonBirYildakiOnaylananToplamTutar = _db.Advances
                        .Where(x => x.AdvanceRequestDate >= DateTime.Now.AddYears(-1) && (x.IsActive && x.IsItConfirmed))
                        .Sum(x => x.AdvancePaymentRequest);

                    var talepYapilabilecekTarih = _db.Advances
                        .Where(x => x.AdvanceRequestDate >= DateTime.Now.AddYears(-1) && (x.IsActive && x.IsItConfirmed))
                        .OrderBy(x => x.AdvanceRequestDate).FirstOrDefault()?.AdvanceRequestDate.AddYears(1);


                    var talepEdilebilecekMaksMiktar = activePersonel.MaxAdvanceLimit - sonBirYildakiOnaylananToplamTutar;


                    var receivedValue = (decimal?)value;

                    if ( receivedValue <= 0)
                    {
                        return new ValidationResult("Avans Miktarı Alanını Geçerli Giriniz.!!!");
                    }
                     if (advanceRequests.Any(x => x.IsItConfirmed == false && x.IsActive == false))
                    {
                        return new ValidationResult("Beklemede bir talebiniz olduğundan yeni avans talep edilemiyor.!!!");
                    }
                     if (talepEdilebilecekMaksMiktar == 0)
                    {
                        return new ValidationResult($"Son Bir Yıl İçindeki Avans Değerinizi Doldurdunuz.{talepYapilabilecekTarih.Value.ToShortDateString()} Tarihine Kadar Yeni Talepte Bulunamazsınız.");
                    }

                    if (vm.AdvancePaymentRequest > GetEmployeeSalaryById(_db, vm.PersonelId) * 3M)
                    {
                        return new ValidationResult("Avans isteği, maaşın 3 katından fazla olamaz!");
                    }
                }
            }

            return ValidationResult.Success;
        }

        private decimal GetEmployeeSalaryById(ApplicationDbContext db, int personelId)
        {
            var employee = db.Personels.FirstOrDefault(e => e.Id == personelId);
            return employee?.Maas ?? 0M;
        }
    }
}
