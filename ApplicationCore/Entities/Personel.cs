using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Personel : BaseEntity
    {
        public string FirstName { get; set; } = null!;

        public string? SecondName { get; set; }

        public string Surname { get; set; } = null!;

        public string? SecondSurname { get; set; }

        public string? Photo { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? BirthPlace { get; set; }

        public string TC { get; set; } = null!;

        public DateTime? StartDateOfWork { get; set; }

        public DateTime? EndDateOfWork { get; set; }

        public string Job { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string MailAdress { get; set; } = null!;

        public bool IsActive { get; set; }

        public bool Gender { get; set; }

        private decimal maas;

        public decimal Maas
        {
            get { return maas; }
            set
            {
                maas = value;
                MaxAdvanceLimit = maas * 3m;
            }
        }

        public decimal MaxAdvanceLimit { get; set; }

        //Navigation Property
        public int? DepartmentId { get; set; }

        public Department? Department { get; set; }

        //Navigation Property
        public int? CompanyId { get; set; }

        public Company? Company { get; set; }

        //Navigation Property
        public int? ManagerId { get; set; }

        public Manager? Manager { get; set; }

        //Navigation Property
        public string? AppUserId { get; set; }

        public ApplicationUser? AppUser { get; set; }

        public List<Advance> Advances { get; set; } = new();

        public List<Permission> Permissions { get; set; } = new();
    }
}
