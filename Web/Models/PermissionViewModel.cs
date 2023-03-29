using ApplicationCore.Entities;
using ApplicationCore.Enums;
using System.ComponentModel.DataAnnotations;
using Web.Validations;

namespace Web.Models
{
    public class PermissionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İzin Tipi Alanı Boş Bırakmayınız.!!!")]
        public PermissionType PermissionType { get; set; }

        public DateTime? StartOfPermissionDate { get; set; }

        public DateTime? EndOfPermissionDate { get; set; }

        public DateTime? RequestDate { get; set; } = DateTime.Now;

        public TimeSpan? CountOfPermittedDays
        {
            get
            {
                return EndOfPermissionDate - StartOfPermissionDate;
            }
        }

        public bool ApprovalState { get; set; }


        public string ApprovalStateText
        {
            get
            {
                if (ApprovalState == true)
                {
                    return "Onaylandı!!";
                }
                else 
                {
                    return "-";
                }
            
            }
        }

        public DateTime? ReplyDate { get; set; }

        public IFormFile? PermissionFile { get; set; }

        public string? PermissionFileUrl { get; set; }

        //Navigation Property
        public int PersonelId { get; set; }

        public Personel Personel { get; set; } = null!;
    }
}
