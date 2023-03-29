using ApplicationCore.Entities;

namespace Web.Models
{
    public class ManagerSummaryViewModel
    {
        public int Id { get; set; }

        public IFormFile? Photo { get; set; }

        public string? PhotoUrl { get; set; }

        public string FirstName { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string MailAdress { get; set; } = null!;

        public string Job { get; set; } = null!;

        public bool Gender { get; set; }

        public string GenderText
        {
            get { return Gender ? "Erkek" : "Kadın"; }
        }

        public List<Department> Departments { get; set; } = new();
    }
}
