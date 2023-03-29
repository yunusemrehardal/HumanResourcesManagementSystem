using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations;
using Web.Validations;

namespace Web.Models
{
    public class AdminViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string? SecondName { get; set; }

        public string Surname { get; set; } = null!;

        public string? SecondSurname { get; set; }

        [PhotoValidation]
        public IFormFile? Photo { get; set; }

        public string? PhotoUrl { get; set; }

        [Required(ErrorMessage = "Doğum Tarihi Alanı Boş Olmaz!!!")]
        [BirthDateValidation(ErrorMessage = "Yaş Aralığı 18 Ve 65 Arasında Olacak Şekilde Doğum Tarihi Alanı Giriniz!!!")]
        public DateTime? BirthDate { get; set; }

        [BirthPlaceValidation]
        public string? BirthPlace { get; set; }

        [Required(ErrorMessage = "TC Alanı Boş Olmaz!!!")]
        [TcValidation(ErrorMessage = "TC Kimlik Numaranız Hatalı")]
        public string TC { get; set; } = null!;

        [Required(ErrorMessage = "Adres Alanı Boş Olmaz!!!")]
        [MaxLength(50, ErrorMessage = "Adres Alanı Maksimum 50 Karakter Olmalıdır."), MinLength(10, ErrorMessage = "Adres Alanı Minimum 10 Karakter Olmalıdır.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Telefon Alanı Boş Olmaz!!!")]
        [PhoneValidation]
        public string PhoneNumber { get; set; } = null!;

        [EMailValidation]
        public string MailAdress { get; set; } = null!;

        [Required(ErrorMessage = "Maas Alanı Boş Olmaz!!!")]
        [MaasValidation]
        public decimal? Maas { get; set; }

        public bool Gender { get; set; }

        public string GenderText
        {
            get { return Gender ? "Erkek" : "Kadın"; }
        }

        public List<Company> Companies { get; set; } = new();

        //Navigation Property
        public string? AppUserId { get; set; }

        public ApplicationUser? AppUser { get; set; }
    }
}
