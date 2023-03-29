
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace ApplicationCore.Services
{
    public class PersoneViewModelService : IPersonelViewModelService
    {
        private readonly IRepository<Personel> _personelRepo;
        private readonly ApplicationDbContext _db;
        private readonly IRepository<Department> _departmanRepo;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Advance> _advanceRepo;

        public PersoneViewModelService(IRepository<Personel> personelRepo, ApplicationDbContext db, IRepository<Department> departmanRepo, IWebHostEnvironment env, UserManager<ApplicationUser> userManager,IRepository<Advance>advanceRepo)
        {
            _personelRepo = personelRepo;
            _db = db;
            _departmanRepo = departmanRepo;
            _env = env;
            _userManager = userManager;
            _advanceRepo = advanceRepo;
        }

        public async Task<PersonelViewModel> GetPersonelViewModelAsync(int personelId)
        {
            var personel = await _personelRepo.GetByIdAsync(personelId);
            _personelRepo.PersonelToAllInclude();
            return MapPersonelToViewModel(personel!);
        }

        public async Task<PersonelViewModel> UpdatePersonelAsync(PersonelViewModel personelViewModel, int personelId)
        {
            var personel = await _personelRepo.GetByIdAsync(personelId);
            if (personel.Photo == null || personel.Photo != null)
            {
                personel.Photo = SavePhoto(personelViewModel.Photo!);
            }
            personel.Address = personelViewModel.Address;
            personel.PhoneNumber = personelViewModel.PhoneNumber;
            await _personelRepo.UpdateAsync(personel);
            return personelViewModel;
        }

        private string SavePhoto(IFormFile photo)
        {
            if (photo == null)
            {
                return null;
            }
            var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
            string filePath = Path.Combine(_env.WebRootPath, "img", fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                photo.CopyTo(stream);
            }
            return fileName;
        }

        public async Task<List<Personel>> ListPersonelAsync()
        {
            _personelRepo.PersonelToAllInclude();
            var personels = _personelRepo.GetAllAsync();
            return await personels;
        }

        public List<Department> BringDepartments()
        {
            return _db.Departments.ToList();
        }

        public List<Manager> BringManagers()
        {
            return _db.Managers.ToList();
        }

        public List<Company> BringCompanies()
        {
            return _db.Companies.ToList();
        }
        public List<Package> BringPackages()
        {
            return _db.Packages.ToList();
        }

        public async Task<Personel> GetPersonelAsync(int personelId)
        {
            var personel = await _personelRepo.GetByIdAsync(personelId);
            _personelRepo.PersonelToAllInclude();
            return personel!;
        }

        public async Task<PersonelSummaryViewModel> GetPersonelSummaryViewModelAsync(int personelId)
        {
            var personel = await _personelRepo.GetByIdAsync(personelId);
            _personelRepo.GetDepartments();
            _personelRepo.PersonelToAllInclude();
            var viewModel = new PersonelSummaryViewModel
            {
                Id = personel.Id,
                FirstName = personel.FirstName,
                Address = personel.Address,
                Department = personel.Department,
                Job = personel.Job,
                MailAdress = personel.MailAdress,
                PhoneNumber = personel.PhoneNumber,
                Surname = personel.Surname,
                PhotoUrl = personel.Photo,
                DepartmentId = (int)personel.DepartmentId,
                Gender = personel.Gender

            };
            _personelRepo.PersonelToAllInclude();
            return viewModel;
        }

        public int GetActivePersonelId(string personelId)
        {
            var activeUser = _userManager.Users.FirstOrDefault(u => u.Id == personelId);
            var activePersonelId = _db.Personels.FirstOrDefault(x => x.AppUserId == activeUser.Id).Id;
            return activePersonelId;
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
                Maas = personel.Maas,
                Advances = personel.Advances.Select(x=>MapAdvanceToViewModel(x)).ToList()
            };
            

            personelViewModel.Company = _db.Companies.FirstOrDefault(x => x.Id == personel.CompanyId);
            personelViewModel.Company.Name = _db.Companies.FirstOrDefault(x => x.Id == personel.CompanyId).Name;
            return personelViewModel;
        }

        public AdvanceViewModel MapAdvanceToViewModel(Advance advance)
        {
            var vm = new AdvanceViewModel
            {
                Id = advance.Id,
                AdvancePaymentRequest = advance.AdvancePaymentRequest,
                AdvanceType = advance.AdvanceType,
                IsActive = advance.IsActive,
                IsItConfirmed = advance.IsItConfirmed,
                //Personel = MapPersonelToViewModel(advance.Personel),
                PersonelId = advance.PersonelId,
                Description = advance.Description
            };
            return vm;
        }



       
    }
}
