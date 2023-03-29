using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Advance:BaseEntity
    {
        public decimal AdvancePaymentRequest { get; set; }
        public decimal RemainingAdvancePaymentRequest { get; set; }
        public decimal TotalAdvancePayment { get; set; }
        public decimal? AdvancePaymentWay { get; set; }
        public decimal? AdvancePaymentAccomodation { get; set; }
        public decimal? AdvancePaymentFood { get; set; }
        public decimal? AdvancePaymentOther { get; set; }
        public DateTime? AdvanceApprovalDate { get; set; }
        public DateTime AdvanceRequestDate { get; set; } = DateTime.Now;    
        public bool AdvanceType { get; set; }
        public bool IsItConfirmed { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
        public Personel Personel { get; set; } = null!;
        public int PersonelId { get; set; }
        public string Currency { get; set; }=null!;
        public string? AdvanceFile { get; set; } 
    }
}
