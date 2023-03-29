using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text;
using Web.Interfaces;
using Web.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace Web.Services
{
    public class ManagerViewModelService : IManagerViewModelService
    {
        private readonly IRepository<Manager> _managerRepo;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;
        private readonly IRepository<Personel> _personelRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagerViewModelService(IRepository<Manager> managerRepo, IWebHostEnvironment env, ApplicationDbContext db, IRepository<Personel> personelRepo, UserManager<ApplicationUser> userManager)
        {
            _managerRepo = managerRepo;
            _env = env;
            _db = db;
            _personelRepo = personelRepo;
            _userManager = userManager;
        }

        public async Task<ManagerViewModel> GetManagerViewModelAsync(int managerId)
        {
            var manager = await _managerRepo.GetByIdAsync(managerId);
            _managerRepo.ManagerToDepartmentInclude();
            _managerRepo.ManagerToCompanyInclude();
            return MapToManagerViewModel(manager!);
        }

        public async Task<ManagerViewModel> UpdateManagerAsync(ManagerViewModel managerViewModel, int managerId)
        {
            var manager = await _managerRepo.GetByIdAsync(managerId);
            if (manager.Photo == null || manager.Photo != null)
            {
                manager.Photo = SavePhoto(managerViewModel.Photo!);
            }

            manager.Address = managerViewModel.Address;
            manager.PhoneNumber = managerViewModel.PhoneNumber;
            await _managerRepo.UpdateAsync(manager);
            return managerViewModel;
        }

        public async Task<ManagerSummaryViewModel> GetSummaryManagerViewModelAsync(int managerId)
        {
            var manager = await _managerRepo.GetByIdAsync(managerId);
            var viewModel = new ManagerSummaryViewModel
            {
                Id = manager.Id,
                FirstName = manager.FirstName,
                Address = manager.Address,
                Departments = manager.Departments,
                Job = manager.Job,
                MailAdress = manager.MailAdress,
                PhoneNumber = manager.PhoneNumber,
                Surname = manager.Surname,
                PhotoUrl = manager.Photo,
                Gender=manager.Gender
            };

            _managerRepo.ManagerToDepartmentInclude();
            return viewModel;
        }

        public async Task AddPersonelViewModelAsync(PersonelViewModel personelViewModel)
        {
            await _personelRepo.AddAsync(MapViewModelToPersonel(personelViewModel));
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

        public string ConvertIFormFileToString(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private void DeletePhoto(string photo)
        {
            if (string.IsNullOrEmpty(photo))
            {
                return;
            }
            string filePath = Path.Combine(_env.WebRootPath, "img", photo);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        public static DepartmentViewModel MapToDepartmentViewModel(Department department)
        {
            var viewModel = new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
                Company = department.Company,
                CompanyId = department.CompanyId,
                Manager = department.Manager,
                ManagerId = department.ManagerId
            };
            return viewModel;
        }

        public ManagerViewModel MapToManagerViewModel(Manager manager)
        {
            var viewModel = new ManagerViewModel
            {
                Id = manager.Id,               
                FirstName = manager.FirstName,
                BirthDate = manager.BirthDate,
                BirthPlace = manager.BirthPlace,
                CompanyId = manager.CompanyId,
                Company = manager.Company,
                EndDateOfWork = manager.EndDateOfWork,
                IsActive = manager.IsActive,
                SecondName = manager.SecondName,
                SecondSurname = manager.SecondSurname,
                StartDateOfWork = manager.StartDateOfWork,
                Gender = manager.Gender,
                TC = manager.TC,
                Address = manager.Address,
                Departments = manager.Departments.Select(MapToDepartmentViewModel).ToList(),
                Job = manager.Job,
                PhoneNumber = manager.PhoneNumber,
                Surname = manager.Surname,
                PhotoUrl = manager.Photo

            };
            _managerRepo.ManagerToDepartmentInclude();
            _managerRepo.ManagerToCompanyInclude();
            viewModel.Company = _db.Companies.FirstOrDefault(x => x.Id == manager.CompanyId);
            viewModel.Company.Name = _db.Companies.FirstOrDefault(x => x.Id == manager.CompanyId).Name;
            //viewModel.MailAdress = _db.Users.FirstOrDefault(x => x.Id == manager.AppUserId).Email;
            return viewModel;
        }

        public static CompanyViewModel MapToCompanyViewModel(Company company)
        {
            var viewModel = new CompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                Address = company.Address,
                PhoneNumber = company.PhoneNumber,
                MailAdress = company.MailAdress,
                LogoUrl = company.Logo,
            };
            return viewModel;
        }

        public PersonelViewModel MapPersonelToViewModel(Personel personel)
        {
            var personelViewModel = new PersonelViewModel
            {
                Id = personel.Id,
                FirstName = personel.FirstName,
                Surname = personel.Surname,
                Address = personel.Address,
                BirthDate = personel.BirthDate,
                BirthPlace = personel.BirthPlace,
                EndDateOfWork = personel.EndDateOfWork,
                IsActive = personel.IsActive,
                Job = personel.Job,
                PhoneNumber = personel.PhoneNumber,
                PhotoUrl = personel.Photo,
                Gender = personel.Gender,
                DepartmentId = (int)personel.DepartmentId!,
                CompanyId = personel.CompanyId,
                Company = personel.Company,
                ManagerId = (int)personel.ManagerId!,
                Manager = personel.Manager
                

            };
            _personelRepo.PersonelToAllInclude();
            personelViewModel.Company = _db.Companies.FirstOrDefault(x => x.Id == personel.Id);
            personelViewModel.Company!.Name = _db.Companies.FirstOrDefault(x => x.Id == personel.Company.Id)!.Name;
            personelViewModel.Manager = _db.Managers.FirstOrDefault(x => x.Id == personel.ManagerId);
            personelViewModel.Manager!.FirstName = _db.Managers.FirstOrDefault(x => x.Id == personel.ManagerId)!.FirstName;

            return personelViewModel;
        }

        public Personel MapViewModelToPersonel(PersonelViewModel personelViewModel)
        {
            _personelRepo.GetDepartments();
            _personelRepo.PersonelToAllInclude();
             
            var personel = new Personel
            {
                Id = personelViewModel.Id,
                FirstName = personelViewModel.FirstName,
                Surname = personelViewModel.Surname,
                Address = personelViewModel.Address,
                BirthPlace = personelViewModel.BirthPlace,
                BirthDate = personelViewModel.BirthDate,
                EndDateOfWork = personelViewModel.EndDateOfWork,
                IsActive = personelViewModel.IsActive,
                Job = personelViewModel.Job,
                MailAdress = personelViewModel.MailAdress,
                PhoneNumber = personelViewModel.PhoneNumber,
                Gender = personelViewModel.Gender,
                DepartmentId = (int)personelViewModel.DepartmentId,
                Department = personelViewModel.Department,
                CompanyId = personelViewModel.CompanyId,
                Company = personelViewModel.Company,
                ManagerId = (int)personelViewModel.ManagerId!,
                Manager = personelViewModel.Manager,
                TC = personelViewModel.TC,
                SecondName = personelViewModel.SecondName,
                SecondSurname = personelViewModel.SecondSurname,
                StartDateOfWork = personelViewModel.StartDateOfWork,
                Maas = personelViewModel.Maas    ,
                
            };

            NameSurnameCheck(personelViewModel, personel);


            _personelRepo.GetDepartments();
            _personelRepo.PersonelToAllInclude();
            personel.Photo = SavePhoto(personelViewModel.Photo);
            return personel;
        }

        private void NameSurnameCheck(PersonelViewModel personelViewModel, Personel personel)
        {
            string email;

            var countsurname = _db.Personels.Where(x => x.Surname == personelViewModel.Surname).Count();
            var countname = _db.Personels.Where(x => x.FirstName == personelViewModel.FirstName).Count();

            if (countname > 0)
            {
                if (countsurname > 0)
                {
                    CharacterControl(personelViewModel);
                    email = $"{personelViewModel.FirstName}.{personelViewModel.Surname}{countsurname}@bilgeadamboost.com";
                }
                else
                {
                    CharacterControl(personelViewModel);
                    email = $"{personelViewModel.FirstName}.{personelViewModel.Surname}@bilgeadamboost.com";
                }
                personel.MailAdress = email.ToLower();

            }
        }

        private static void CharacterControl(PersonelViewModel personelViewModel)
        {
            string turkishCharacters = "ıçöğüş";
            string englishCharacters = "icogus";
            for (int i = 0; i < turkishCharacters.Length; i++)
            {
                personelViewModel.FirstName = personelViewModel.FirstName.Replace(turkishCharacters[i], englishCharacters[i]);
                personelViewModel.Surname = personelViewModel.Surname.Replace(turkishCharacters[i], englishCharacters[i]);
            }
        }

        public int GetActiveManagerId(string id)
        {
            var activeUser = _userManager.Users.FirstOrDefault(u => u.Id == id);
            var activeManagerId = _db.Managers.FirstOrDefault(x => x.AppUserId == activeUser.Id).Id;
            return activeManagerId;
        }

        public List<Manager> GetAllManagers()
        {
            var managers = _db.Managers.ToList();
            return managers;
        }
    }
}
