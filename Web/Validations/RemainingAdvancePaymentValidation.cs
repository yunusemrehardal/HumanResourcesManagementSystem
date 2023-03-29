using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Web.Validations
{
    public class RemainingAdvancePaymentValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
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
                var advanceRequests = _db.Advances.Where(x => x.PersonelId == activePersonel.Id).ToList();

                if (value != null && value is decimal remainingAdvancePaymentRequest)
                {
                    remainingAdvancePaymentRequest = activePersonel.MaxAdvanceLimit;
                    if (remainingAdvancePaymentRequest < 0)
                    {
                        return new ValidationResult("Kalan avans tutarını aştınız!");
                    }

                    var advanceRequestsTotal = activePersonel.MaxAdvanceLimit;
                    var approvedAdvancePaymentTotal = _db.Advances.Where(x => x.PersonelId == activePersonel.Id && x.IsActive == true && x.IsItConfirmed == true).Sum(x => x.AdvancePaymentRequest);
                    var remainingAdvancePayment = 0M; 

                    if (remainingAdvancePayment == 0)
                    {
                        remainingAdvancePayment=  advanceRequestsTotal - approvedAdvancePaymentTotal;
                    }
                    if (remainingAdvancePayment != 0)
                    {
                        remainingAdvancePayment = remainingAdvancePayment - approvedAdvancePaymentTotal;
                    }
                    if (remainingAdvancePayment > remainingAdvancePaymentRequest)
                    {
                        return new ValidationResult($"Kalan avans tutarı, toplam avans tutarından fazla olamaz! Toplam avans tutarı: {remainingAdvancePayment}");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
