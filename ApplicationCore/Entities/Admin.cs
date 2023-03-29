using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Admin:BaseEntity
    {
        public string FirstName { get; set; } = null!;

        public string? SecondName { get; set; }

        public string Surname { get; set; } = null!;

        public string? SecondSurname { get; set; }

        public string? Photo { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? BirthPlace { get; set; }

        public string TC { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string MailAdress { get; set; } = null!;

        public bool Gender { get; set; }

        public List<Company> Companies { get; set; } = new();

        //Navigation Property
        public string? AppUserId { get; set; }

        public ApplicationUser? AppUser { get; set; }
    }
}
