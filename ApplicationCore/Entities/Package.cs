using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Package : BaseEntity
    {
        public string Name { get; set; } = null!;

      

        public decimal? Price { get; set; }

        public DateTime? StartDateOfRelease { get; set; }

        public DateTime? EndDateOfRelease { get; set;}

        public int PackageUserCount { get; set; }

        public bool IsActive { get; set; }

        public string? Currency { get; set; }

        public List<Company> Companies { get; set; } = new();
    }
}
