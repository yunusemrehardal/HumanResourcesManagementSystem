using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Permission : BaseEntity
    {
        public PermissionType PermissionType { get; set; }

        public DateTime? StartOfPermissionDate { get; set; }

        public DateTime? EndOfPermissionDate { get;  set; }

        public DateTime RequestDate  { get; set; } = DateTime.Now;

        public TimeSpan? CountOfPermittedDays { get; set; }

        public bool ApprovalState { get; set; }

        public DateTime? ReplyDate { get; set; }

        public string? PermissionFile { get; set; }

        //Navigation Property
        public int PersonelId { get; set; }

        public Personel Personel { get; set; } = null!;
    }
}
