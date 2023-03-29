using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class PackageViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Paket Adı zorunludur.")]
        [MinLength(2, ErrorMessage = "Paket Adı en az 2 karakter olmalıdır.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan küçük olamaz.")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Yayınlanma Tarihi zorunludur.")]
        [DataType(DataType.Date)]   
        public DateTime? StartDateOfRelease { get; set; }

        public DateTime? EndDateOfRelease { get; set; }

        public int PackageUserCount { get; set; }

        public bool IsActive { get; set; }

        public string? Currency { get; set; }

        public List<CompanyViewModel> Companies { get; set; } = new();
    }
}
