using ApplicationCore.Entities;

namespace Web.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        //Navigation Property
        public int ManagerId { get; set; }

        public Manager Manager { get; set; }

        //Navigation Property
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        //Navigation Property
        public int PersonelId { get; set; }

        public Personel Personel { get; set; }
    }
}
