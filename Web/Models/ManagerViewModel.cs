using ApplicationCore.Entities;
using Web.Validations;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ManagerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim Alanı Boş Olmaz!!!")]
        [FirstNamesValidation(ErrorMessage = "İsim Alanı Sadece Harflerden Oluşabilir!!!")]
        public string FirstName { get; set; } = null!;

        [SecondNamesValidation(ErrorMessage = "İkinci İsim Alanı Sadece Harflerden Oluşabilir!!!")]
        public string? SecondName { get; set; }

        [Required(ErrorMessage = "Soyad Alanı Boş Olmaz!!!")]
        [SurnameValidation(ErrorMessage = "Soyad Alanı Sadece Harflerden Oluşabilir!!!")]
        public string Surname { get; set; } = null!;

        [SecondSurnameValidation(ErrorMessage = "İkinci Soyad Alanı Sadece Harflerden Oluşabilir!!!")]
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

        [StartDateOfWorkValidation]
        public DateTime? StartDateOfWork { get; set; }

        [EndDateOfWorkValidation]
        public DateTime? EndDateOfWork { get; set; }

        [Required(ErrorMessage = "Meslek Alanı Boş Olmaz!!!")]
        [JobValidation(ErrorMessage = "Meslek Alanı Sadece Harflerden Oluşabilir!!!")]
        public string Job { get; set; } = null!;

        [Required(ErrorMessage = "Adres Alanı Boş Olmaz!!!")]
        [MaxLength(50, ErrorMessage = "Adres Alanı Maksimum 50 Karakter Olmalıdır."), MinLength(10, ErrorMessage = "Adres Alanı Minimum 10 Karakter Olmalıdır.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Telefon Alanı Boş Olmaz!!!")]
        [PhoneValidation]
        public string PhoneNumber { get; set; } = null!;

        [EMailValidation(ErrorMessage = "@bilgeadamboost.com formatında olmalıdır!!!")]
        public string MailAdress
        {
            get
            {
                string email;
                if (FirstName != null && Surname != null)
                {
                    email = $"{FirstName.ToLower()}";

                    if (!string.IsNullOrEmpty(Surname))
                    {
                        email += $".{Surname.ToLower()}";
                    }

                    string turkishCharacters = "ıçöğüş";
                    string englishCharacters = "icogus";
                    for (int i = 0; i < turkishCharacters.Length; i++)
                    {
                        email = email.Replace(turkishCharacters[i], englishCharacters[i]);
                    }
                    email += "@bilgeadamboost.com";
                    return email;
                }
                email = "";
                return email;
            }
        }

        public bool IsActive { get; set; }

        public bool Gender { get; set; }

        public string GenderText
        {
            get { return Gender ? "Erkek" : "Kadın"; }
        }

        private decimal maas;

        [Required(ErrorMessage = "Maas Alanı Boş Olmaz!")]
        [MaasValidation]
        public decimal Maas
        {
            get { return maas; }
            set
            {
                maas = value;
                MaxAdvanceLimit = (maas * 3m);
            }
        }

        public decimal MaxAdvanceLimit { get; set; }

        public List<DepartmentViewModel> Departments { get; set; } = new();

        //Navigation Property
        public int? CompanyId { get; set; }

        public Company? Company { get; set; }

        //Navigation Property
        public string? AppUserId { get; set; }

        public ApplicationUser? AppUser { get; set; }
    }
}
