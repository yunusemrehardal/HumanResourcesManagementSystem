using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations;
using Web.Validations;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using Infrastructure.Data;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Web.Models
{
    public class AdvanceViewModel
    {
        public int Id { get; set; }

        [AdvancePaymentValidation]
        [RemainingAdvancePaymentValidation]
        [Required(ErrorMessage = "Avans Talep Etme Alanı Boş Bırakmayınız.!!!")]
        public decimal? AdvancePaymentRequest { get; set; } 

        public DateTime AdvanceRequestDate { get; set; } = DateTime.Now;

        public bool AdvanceType { get; set; }
        public string AdvanceTypeText
        {
            get { return AdvanceType ? "Kurumsal" : "Bireysel"; }
        }

        [Required(ErrorMessage = "Açıklama Alanı Zorunludur.!!!")]
        [MaxLength(100, ErrorMessage = "100 karakterden fazla karakter girişi yapılamaz.!!!")]
        public string? Description { get; set; }

        public bool IsItConfirmed { get; set; }

        public string IsItConfirmedText
        {
            get
            {
                if (IsActive == true && IsItConfirmed == true)
                {
                    return "Onaylandı";
                }
                else if (IsActive == true && IsItConfirmed == false)
                {
                    return "Reddedildi!!";
                }
                else
                {
                    return "Onay Bekliyor";
                }
            }
        }

        public bool IsActive { get; set; }

        public Personel Personel { get; set; } = null!;

        public int PersonelId { get; set; }
        //public PersonelViewModel Personel { get; set; }

        private decimal _remainingAdvancePaymentRequest;
        public decimal RemainingAdvancePaymentRequest
        {
            get => _remainingAdvancePaymentRequest;
            set
            {
                if (IsActive && IsItConfirmed && _remainingAdvancePaymentRequest == 0M)
                {
                    _remainingAdvancePaymentRequest = (decimal)(Personel.MaxAdvanceLimit  - AdvancePaymentRequest);
                }
                if (IsActive && IsItConfirmed && _remainingAdvancePaymentRequest != 0M)
                {
                    _remainingAdvancePaymentRequest = (decimal)(_remainingAdvancePaymentRequest - AdvancePaymentRequest);
                }   
                else
                {
                    _remainingAdvancePaymentRequest = value;
                }
            }
        }

        [Required(ErrorMessage ="Para Birimi Alanı Zorunludur")]
        public string Currency { get; set; } = null!;

        public IFormFile AdvanceFile { get; set; } = null!;

        public string? AdvanceFileUrl { get; set; }

        public decimal TotalAdvancePayment { get; set; }

        public decimal? AdvancePaymentWay { get; set; }

        public decimal? AdvancePaymentAccomodation { get; set; }

        public decimal? AdvancePaymentFood { get; set; }

        public decimal? AdvancePaymentOther { get; set; }

        public DateTime? AdvanceApprovalDate { get; set; }
    }
}
