using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
//using ApplicationCore.Specifications;
using Ardalis.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;
using Web.Interfaces;
using Web.Models;

namespace Web.Services
{
    public class AdminViewModelService : IAdminViewModelService
    {
        private readonly IRepository<Admin> _adminRepo;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;
        private readonly IRepository<Company> _companyRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Manager> _managerRepo;
        private readonly IRepository<Package> _packageRepo;

        public AdminViewModelService(IRepository<Admin> adminRepo, IWebHostEnvironment env, ApplicationDbContext db, IRepository<Company> companyRepo, UserManager<ApplicationUser> userManager, IRepository<Manager> managerRepo,IRepository<Package> packageRepo)
        {
            _adminRepo = adminRepo;
            _env = env;
            _db = db;
            _companyRepo = companyRepo;
            _userManager = userManager;
            _managerRepo = managerRepo;
            _packageRepo = packageRepo;
        }

        public async Task AddCompanyViewModelAsync(CompanyViewModel companyViewModel)
        {
            await _companyRepo.AddAsync(ViewModelToCompany(companyViewModel));
        }

        public int GetActiveAdminId(string adminId)
        {
            var activeUser = _userManager.Users.FirstOrDefault(u => u.Id == adminId);
            var activeAdminId = _db.Admins.FirstOrDefault(x => x.AppUserId == activeUser.Id).Id;
            return activeAdminId;
        }

        public async Task<AdminViewModel> GetAdminViewModelAsync(int adminId)
        {
            var admin = await _adminRepo.GetByIdAsync(adminId);
            return MapToAdminViewModel(admin!);
        }

        public async Task<AdminViewModel> UpdateAdminAsync(AdminViewModel adminViewModel, int adminId)
        {
            var admin = await _adminRepo.GetByIdAsync(adminId);
            if (admin.Photo == null || admin.Photo != null)
            {
                admin.Photo = SavePhoto(adminViewModel.Photo!);
            }

            admin.Address = adminViewModel.Address;
            admin.PhoneNumber = adminViewModel.PhoneNumber;
            await _adminRepo.UpdateAsync(admin);
            return adminViewModel;
        }

        public AdminViewModel MapToAdminViewModel(Admin admin)
        {
            var viewModel = new AdminViewModel
            {
                Id = admin.Id,
                Address = admin.Address,
                AppUser = admin.AppUser,
                AppUserId = admin.AppUserId,
                BirthDate = admin.BirthDate,
                BirthPlace = admin.BirthPlace,
                Companies = admin.Companies,
                FirstName = admin.FirstName,
                Gender = admin.Gender,
                MailAdress = admin.MailAdress,
                PhoneNumber = admin.PhoneNumber,
                PhotoUrl = admin.Photo,
                SecondName = admin.SecondName,
                SecondSurname = admin.SecondSurname,
                Surname = admin.Surname,
                TC = admin.TC
            };
            return viewModel;
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

        public CompanyViewModel MapToCompanyViewModel(Company company)
        {
            var viewModel = new CompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                Address = company.Address,
                PhoneNumber = company.PhoneNumber,
                MailAdress = company.MailAdress,
                LogoUrl = company.Logo,
                Departments = company.Departments?.Select(MapToDepartmentViewModel).ToList(),
                ContractStartYear = company.ContractStartYear,
                ContractEndYear = company.ContractEndYear,
                FoundationYear = company.FoundationYear,
                IsActive = company.IsActive,
                MersisNo = company.MersisNo,
                TaxDepartment = company.TaxDepartment,
                Managers = (company.Managers)?.Select(m => MapToManagerViewModel(m)).ToList(),
                TaxNo = company.TaxNo,
                Title = company.Title,
                PackageId= (int)company.PackageId,
                EndDateOfPackage = company.EndDateOfPackage,
                StartDateOfPackage = company.StartDateOfPackage,
               

            };
           // viewModel.Package = _db.Packages.FirstOrDefault(x => x.Id == company.PackageId);
            return viewModel;
        }

        public Company ViewModelToCompany(CompanyViewModel companyViewModel)
        {
            var company = new Company
            {
                Id = companyViewModel.Id,
                Name = companyViewModel.Name,
                Address = companyViewModel.Address,
                PhoneNumber = companyViewModel.PhoneNumber,
                MailAdress = companyViewModel.MailAdress,
                Departments = companyViewModel.Departments?.Select(MapViewModelToDepartment).ToList(),
                ContractStartYear = companyViewModel.ContractStartYear,
                ContractEndYear = companyViewModel.ContractEndYear,
                FoundationYear = companyViewModel.FoundationYear,
                IsActive = companyViewModel.IsActive,
                MersisNo = companyViewModel.MersisNo,
                TaxDepartment = companyViewModel.TaxDepartment,
                Managers = companyViewModel.Managers?.Select(m => MapViewModelToManager(m)).ToList(),
                TaxNo = companyViewModel.TaxNo,
                Title = companyViewModel.Title,
                Logo = companyViewModel.LogoUrl,
                PackageId= companyViewModel.PackageId,
                StartDateOfPackage = companyViewModel.StartDateOfPackage,
                RemainingPackageTime = companyViewModel.RemainingPackageTime,
                EndDateOfPackage = companyViewModel.EndDateOfPackage,
                
                
            };
                company.Package= _db.Packages.FirstOrDefault(x=>x.Id == company.PackageId);
                 company.Package.Name = _db.Packages.First(x => x.Id == company.PackageId).Name;
                company.Logo = SavePhoto(companyViewModel.Logo);
            return company;
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

        public static Department MapViewModelToDepartment(DepartmentViewModel departmentViewModel)
        {
            var department = new Department
            {
                Id = departmentViewModel.Id,
                Name = departmentViewModel.Name,
                Company = departmentViewModel.Company,
                CompanyId = departmentViewModel.CompanyId,
                Manager = departmentViewModel.Manager,
                ManagerId = departmentViewModel.ManagerId
            };
            return department;
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

        public Manager MapViewModelToManager(ManagerViewModel managerViewModel)
        {
            var manager = new Manager
            {
                Id = managerViewModel.Id,
                FirstName = managerViewModel.FirstName,

                BirthDate = managerViewModel.BirthDate,
                BirthPlace = managerViewModel.BirthPlace,
                CompanyId = managerViewModel.CompanyId,
                Company = managerViewModel.Company,
                EndDateOfWork = managerViewModel.EndDateOfWork,
                IsActive = managerViewModel.IsActive,
                SecondName = managerViewModel.SecondName,
                SecondSurname = managerViewModel.SecondSurname,
                StartDateOfWork = managerViewModel.StartDateOfWork,
                Gender = managerViewModel.Gender,
                TC = managerViewModel.TC,
                Address = managerViewModel.Address,
                Departments = managerViewModel.Departments.Select(MapViewModelToDepartment).ToList(),
                Job = managerViewModel.Job,
                PhoneNumber = managerViewModel.PhoneNumber,
                Surname = managerViewModel.Surname,
                Photo = managerViewModel.PhotoUrl,
                MailAdress=managerViewModel.MailAdress,
                Maas = managerViewModel.Maas
            };

            NameSurnameCheck(managerViewModel, manager);

            _managerRepo.ManagerToDepartmentInclude();
            _managerRepo.ManagerToCompanyInclude();
            manager.Photo = SavePhoto(managerViewModel.Photo!);
            return manager;
        }

        public Package MapViewModelToPackage(PackageViewModel packageViewModel)
        {
            var package = new Package
            { 
                Id = packageViewModel.Id,
                Name = packageViewModel.Name,
                Currency = packageViewModel.Currency,
         
                EndDateOfRelease= packageViewModel.EndDateOfRelease,
                IsActive= packageViewModel.IsActive,
                PackageUserCount= packageViewModel.PackageUserCount,
                Price=packageViewModel.Price,
                StartDateOfRelease= packageViewModel.StartDateOfRelease,
                
                Companies = packageViewModel.Companies.Select(x=> ViewModelToCompany(x)).ToList()
            };
            return package;
        }
        private void NameSurnameCheck(ManagerViewModel managerViewModel, Manager manager)
        {
            string email;

            var countsurname = _db.Managers.Where(x => x.Surname == managerViewModel.Surname).Count();
            var countname = _db.Managers.Where(x => x.FirstName == managerViewModel.FirstName).Count();

            if (countname > 0)
            {
                if (countsurname > 0)
                {
                    email = $"{managerViewModel.FirstName}.{managerViewModel.Surname}{countsurname}@bilgeadamboost.com";

                }
                else
                {
                    email = $"{managerViewModel.FirstName}.{managerViewModel.Surname}@bilgeadamboost.com";
                }
                manager.MailAdress = email.ToLower();

            }
        }

        public async Task<AdminSummaryViewModel> GetAdminSummaryViewModelAsync(int adminId)
        {
            var admin = await _adminRepo.GetByIdAsync(adminId);
            var viewModel = new AdminSummaryViewModel
            {
                Id = admin.Id,
                FirstName = admin.FirstName,
                Address = admin.Address,
                MailAdress = admin.MailAdress,
                PhoneNumber = admin.PhoneNumber,
                Surname = admin.Surname,
                PhotoUrl = admin.Photo,
                Gender = admin.Gender
            };
            return viewModel;
        }

        public async Task<List<Company>> ListCompanyAsync()
        {
            var companies = _companyRepo.GetAllAsync();
            return await companies;
        }

        public async Task<CompanyViewModel> UpdateCompanyAsync(CompanyViewModel companyViewModel, int id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            company.Logo = SavePhoto(companyViewModel.Logo!);
            company.Address = companyViewModel.Address;
            company.PhoneNumber = companyViewModel.PhoneNumber;
            company.MailAdress = companyViewModel.MailAdress;
            company.Title = companyViewModel.Title;
            company.IsActive = companyViewModel.IsActive;
            company.ContractStartYear = companyViewModel.ContractStartYear;
            company.ContractEndYear = companyViewModel.ContractEndYear;
            company.PackageId = companyViewModel.PackageId;
            var package = _db.Packages.FirstOrDefault(x=>x.Id == company.PackageId);
            company.Package = package;
            company.StartDateOfPackage = companyViewModel.StartDateOfPackage;
            company.EndDateOfPackage = companyViewModel.EndDateOfPackage;
            company.RemainingPackageTime = companyViewModel.RemainingPackageTime;
            await _companyRepo.UpdateAsync(company);
            return companyViewModel;
        }

        public async Task<CompanyViewModel> GetCompanyViewModelAsync(int id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            var vm = MapToCompanyViewModel(company!);
            return vm;
        }

        public async Task AddManagerViewModelAsync(ManagerViewModel managerViewModel)
        {
            await _managerRepo.AddAsync(MapViewModelToManager(managerViewModel));
        }

        public List<Manager> GetManagersBySurname(string surname)
        {
            return _db.Managers.Where(m => m.Surname == surname).ToList();
        }

        public async Task AddPackageViewModelAsync(PackageViewModel packageViewModel)
        {
            await _packageRepo.AddAsync(MapViewModelToPackage(packageViewModel));
        }

        public async Task<List<Package>> ListPackagesAsync()
        {
            var packages = _packageRepo.GetAllAsync();
            return await packages;
        }
    }
}
