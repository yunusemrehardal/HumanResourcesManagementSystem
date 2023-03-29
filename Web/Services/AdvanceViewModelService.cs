using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Web.Interfaces;
using Web.Models;

namespace Web.Services
{
    public class AdvanceViewModelService : IAdvanceViewModelService
    {
        private readonly ApplicationDbContext _db;
        private readonly IRepository<Advance> _advanceRepo;
        private readonly IRepository<Personel> _personelRepo;
        private readonly IPersonelViewModelService _personelViewModelService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AdvanceViewModelService(ApplicationDbContext db, IRepository<Advance> advanceRepo, IRepository<Personel> personelRepo, IPersonelViewModelService personelViewModelService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _db = db;
            _advanceRepo = advanceRepo;
            _personelRepo = personelRepo;
            _personelViewModelService = personelViewModelService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _env = env;
        }

        public async Task AdvanceRequestAsync(AdvanceViewModel vm)
        {
             await  _advanceRepo.AddAsync(ViewModelToAdvance(vm));
        }

        public Task<List<AdvanceViewModel>> GetPersonelAdvances(int personelId)
        {
           return (Task<List<AdvanceViewModel>>)_db.Advances.ToList().Where(x=>x.Id == personelId);
        }


        public AdvanceViewModel MapAdvanceToViewModel(Advance advance)
        {
            var personelReal = _db.Personels.Include(p => p.Advances).FirstOrDefault(p => p.Id == advance.PersonelId);

            var vm = new AdvanceViewModel
            {
                Id = advance.Id,
                AdvancePaymentRequest = advance.AdvancePaymentRequest,
                AdvanceType= advance.AdvanceType,
                IsActive= advance.IsActive,
                IsItConfirmed= advance.IsItConfirmed,
                Personel = personelReal,
                PersonelId = advance.PersonelId,
                Description = advance.Description,
                Currency = advance.Currency,
                AdvanceFileUrl = advance.AdvanceFile,
                AdvanceApprovalDate= advance.AdvanceApprovalDate,
                AdvancePaymentAccomodation=advance.AdvancePaymentAccomodation,
                AdvancePaymentFood= advance.AdvancePaymentFood,
                AdvancePaymentOther= advance.AdvancePaymentOther,
                AdvancePaymentWay= advance.AdvancePaymentWay,
                AdvanceRequestDate= advance.AdvanceRequestDate,
                RemainingAdvancePaymentRequest= advance.RemainingAdvancePaymentRequest,
                TotalAdvancePayment=advance.TotalAdvancePayment
            };
            return vm;
        }

        private string SaveFile(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_env.WebRootPath, "downloads", fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }

        public string YukleAsync(IFormFile dosya)
        {
            string dosyaAdi = Path.GetFileNameWithoutExtension(dosya.FileName);
            string dosyaUzantisi = Path.GetExtension(dosya.FileName);
            string yeniDosyaAdi = $"{dosyaAdi}_{DateTime.UtcNow.Ticks}{dosyaUzantisi}";
            string yuklemeYolu = Path.Combine(_env.WebRootPath, "downloads", yeniDosyaAdi);

            using (var dosyaAkisi = new FileStream(yuklemeYolu, FileMode.Create))
            {
                dosya.CopyToAsync(dosyaAkisi);
            }

            return yeniDosyaAdi;
        }
        public string ConvertIFormFileToString(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private void DeleteFile(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return;
            }
            string filePath = Path.Combine(_env.WebRootPath, "img", file);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        private string GetUserId()
        {
            var httpContextUser = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            return httpContextUser;
        }


        public Advance ViewModelToAdvance(AdvanceViewModel advanceViewModel)
        {
            var personel = _db.Personels.FirstOrDefault(x => x.AppUserId == GetUserId()).Id;
            advanceViewModel.PersonelId = personel;
            var personelReal = _db.Personels.Include(p => p.Advances).FirstOrDefault(p => p.Id == advanceViewModel.PersonelId);
            var advance = new Advance
            {
                Id = advanceViewModel.Id,
                AdvancePaymentRequest = (decimal)advanceViewModel.AdvancePaymentRequest,
                AdvanceType = advanceViewModel.AdvanceType,
                IsActive = advanceViewModel.IsActive,
                IsItConfirmed = advanceViewModel.IsItConfirmed,
                Personel = personelReal,
                PersonelId = advanceViewModel.PersonelId,
                Description = advanceViewModel.Description,
                Currency = advanceViewModel.Currency,
                AdvanceFile = advanceViewModel.AdvanceFileUrl,
                AdvanceApprovalDate = advanceViewModel.AdvanceApprovalDate,
                AdvancePaymentAccomodation = advanceViewModel.AdvancePaymentAccomodation,
                AdvancePaymentFood = advanceViewModel.AdvancePaymentFood,
                AdvancePaymentOther = advanceViewModel.AdvancePaymentOther,
                AdvancePaymentWay = advanceViewModel.AdvancePaymentWay,
                AdvanceRequestDate = advanceViewModel.AdvanceRequestDate,
                RemainingAdvancePaymentRequest = advanceViewModel.RemainingAdvancePaymentRequest,
                TotalAdvancePayment = advanceViewModel.TotalAdvancePayment
            };
            if (advance.AdvanceFile != null)
            {
                advance.AdvanceFile = YukleAsync(advanceViewModel.AdvanceFile);
            }

            return advance;
        }
        public Personel MapViewModelToPersonel(PersonelViewModel personelViewModel)
        {
            _personelRepo.GetDepartments();
            _personelRepo.PersonelToAllInclude();
            var personel = new Personel
            {
                Id = personelViewModel.Id,
                FirstName = personelViewModel.FirstName,
                TC = personelViewModel.TC,
                Photo = personelViewModel.PhotoUrl,
                SecondSurname = personelViewModel.SecondSurname,
                StartDateOfWork = personelViewModel.StartDateOfWork,
                SecondName = personelViewModel.SecondName,
                Surname = personelViewModel.Surname,
                Address = personelViewModel.Address,
                BirthDate = personelViewModel.BirthDate,
                BirthPlace = personelViewModel.BirthPlace,
                EndDateOfWork = personelViewModel.EndDateOfWork,
                IsActive = personelViewModel.IsActive,
                Job = personelViewModel.Job,
                MailAdress = personelViewModel.MailAdress,
                PhoneNumber = personelViewModel.PhoneNumber,
                Gender = personelViewModel.Gender,
                DepartmentId = (int)personelViewModel.DepartmentId!,
                Department = personelViewModel.Department,
                CompanyId = personelViewModel.CompanyId,
                Company = personelViewModel.Company,
                ManagerId = (int)personelViewModel.ManagerId!,
                Manager = personelViewModel.Manager,
                Maas = personelViewModel.Maas,
                Advances = personelViewModel.Advances.Select(x => ViewModelToAdvance(x)).ToList(),

            };
            personel.Company = _db.Companies.FirstOrDefault(x => x.Id == personelViewModel.CompanyId);
            personel.Company.Name = _db.Companies.FirstOrDefault(x => x.Id == personelViewModel.CompanyId).Name;
            return personel;
        }
        public PersonelViewModel MapPersonelToViewModel(Personel personel)
        {
            _personelRepo.GetDepartments();
            _personelRepo.PersonelToAllInclude();
            var personelViewModel = new PersonelViewModel
            {
                Id = personel.Id,
                FirstName = personel.FirstName,
                TC = personel.TC,
                PhotoUrl = personel.Photo,
                SecondSurname = personel.SecondSurname,
                StartDateOfWork = personel.StartDateOfWork,
                SecondName = personel.SecondName,
                Surname = personel.Surname,
                Address = personel.Address,
                BirthDate = personel.BirthDate,
                BirthPlace = personel.BirthPlace,
                EndDateOfWork = personel.EndDateOfWork,
                IsActive = personel.IsActive,
                Job = personel.Job,
                PhoneNumber = personel.PhoneNumber,
                Gender = personel.Gender,
                DepartmentId = (int)personel.DepartmentId!,
                Department = personel.Department,
                CompanyId = personel.CompanyId,
                Company = personel.Company,
                ManagerId = (int)personel.ManagerId!,
                Manager = personel.Manager,
                


            };
            personelViewModel.Company = _db.Companies.FirstOrDefault(x => x.Id == personel.CompanyId);
            personelViewModel.Company.Name = _db.Companies.FirstOrDefault(x => x.Id == personel.CompanyId).Name;
            return personelViewModel;
        }

        public async Task DeleteAdvance(int id)
        {
            var advance = await _advanceRepo.GetByIdAsync(id);
            await _advanceRepo.DeleteAsync(advance);
        }

        public Task FindPersonel(int personelId)
        {
            var personel = _advanceRepo.FirstOrDefaultAsync(personelId);
            return personel;
        }

        public async Task<AdvanceViewModel> UpdateAdvanceAsync(AdvanceViewModel advanceViewModel)
        {
            var advance = await _advanceRepo.GetByIdAsync(advanceViewModel.Id);


            advance.AdvanceFile = advanceViewModel.AdvanceFileUrl;

            await _advanceRepo.UpdateAsync(advance);
            return advanceViewModel;

        }

        public async Task<AdvanceViewModel> GetByIdAsync(int id)
        {
            return await _db.FindAsync<AdvanceViewModel>(id);
        }
    }
}
