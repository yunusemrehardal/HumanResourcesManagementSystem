using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations;
using Web.Validations;

namespace Web.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim Alanı Boş Olmaz!!!")]
        [CompanyNameValidation(ErrorMessage = "İsim Alanı Sadece Harflerden ve Rakamladan Oluşabilir!!!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Adres Alanı Boş Olmaz!!!")]
        [MaxLength(50, ErrorMessage = "Adres Alanı Maksimum 50 Karakter Olmalıdır."), MinLength(10, ErrorMessage = "Adres Alanı Minimum 10 Karakter Olmalıdır.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Telefon Alanı Boş Olmaz!!!")]
        [CompanyPhoneValidation]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "E-Mail Alanı Boş Olmaz!!!")]
        public string MailAdress { get; set; } = null!;

        [PhotoValidation]
        public IFormFile? Logo { get; set; }

        public string? LogoUrl { get; set; }

        public List<ManagerViewModel>? Managers { get; set; }

        public List<DepartmentViewModel>? Departments { get; set; }

        [Required(ErrorMessage = "Unvan Alanı Boş Olmaz!!!")]
        [TitleValidation]

        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Mersis No Alanı Boş Olmaz!!!")]
        [MaxLength(16, ErrorMessage = "Mersis No 16 haneli olmalıdır"), MinLength(16, ErrorMessage = "Mersis No 16 haneli olmalıdır")]
        public string MersisNo { get; set; } = null!;

        [Required(ErrorMessage = "Vergi No Alanı Boş Olmaz!!!")]
        [MaxLength(10, ErrorMessage = "Vergi No 10 haneli olmalıdır"), MinLength(10, ErrorMessage = "Vergi No 10 haneli olmalıdır")]
        public string TaxNo { get; set; } = null!;

        [Required(ErrorMessage = "Vergi Dairesi Alanı Boş Olmaz!!!")]
        [FirstNamesValidation(ErrorMessage = "Vergi Dairesi Alanı Sadece Harflerden Oluşabilir!!!")]
        public string TaxDepartment { get; set; } = null!;

        [Required(ErrorMessage = "Kuruluş Yılı Alanı Boş Olmaz!!!")]
        [FoundationYearValidation(ErrorMessage = $"Kuruluş Yılını lütfen 1800 ve günümüz yılı arasında giriniz.")]
        public DateTime? FoundationYear { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Sözleşme Başlama Tarihi Alanı Boş Olmaz!!!")]
        [ContractStartYearValidation]
        public DateTime? ContractStartYear { get; set; }

        [ContractEndYearValidation]
        public DateTime? ContractEndYear { get; set; }
        public int PackageId { get; set; }
        public Package? Package { get; set; }
        public DateTime? StartDateOfPackage { get; set; }

        public DateTime? EndDateOfPackage { get; set; }


        public TimeSpan? RemainingPackageTime
        {
            get
            {
                return EndDateOfPackage - StartDateOfPackage;
            }
        }
    }
}
