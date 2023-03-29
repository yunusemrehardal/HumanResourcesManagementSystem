using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Department:BaseEntity
    {
        public string Name { get; set; } = null!;

        //Navigation Property
        public int ManagerId { get; set; }

        public Manager Manager { get; set; }

        //Navigation Property
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public List<Personel> Personels { get; set; } = new();
    }
}
