using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string MailAdress { get; set; } = null!;

        public string? Logo { get; set; }

        public List<Manager>? Managers { get; set; } = new();

        public List<Department>? Departments { get; set; } = new();

        public List<Personel>? Personels { get; set; } = new();

        //Navigation Property
        public Admin? Admin { get; set; }

        public int? AdminId { get; set; }

        public string Title { get; set; } = null!;

        public string MersisNo { get; set; } = null!;

        public string TaxNo { get; set; } = null!;

        public string TaxDepartment { get; set; }=null!;

        public DateTime? FoundationYear { get; set; }

        public bool IsActive { get; set; }

        public DateTime? ContractStartYear { get; set; }

        public DateTime? ContractEndYear { get; set; }
        public int? PackageId { get; set; }
        public Package? Package { get; set; }
        public DateTime? StartDateOfPackage { get; set; }

        public DateTime? EndDateOfPackage { get; set; }

        public TimeSpan? RemainingPackageTime { get; set; }
    }
}
