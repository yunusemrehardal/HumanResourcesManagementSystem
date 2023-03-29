using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net.Mail;
using System.Net;
using Web.Interfaces;
using Web.Models;
using Web.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IManagerViewModelService _managerViewModelService;
        private readonly IWebHostEnvironment _env;
        private readonly IPersonelViewModelService _personelViewModelService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminViewModelService _adminViewModelService;
        private readonly IRepository<Manager> _managerRepo;
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _service;

        public AdminController(IManagerViewModelService managerViewModelService, IWebHostEnvironment env, IPersonelViewModelService personelViewModelService, IEmailSender service, UserManager<ApplicationUser> userManager, IAdminViewModelService adminViewModelService, IRepository<Manager> managerRepo, ApplicationDbContext db)
        {
            _managerViewModelService = managerViewModelService;
            _env = env;
            _personelViewModelService = personelViewModelService;
            _service = service;
            _userManager = userManager;
            _adminViewModelService = adminViewModelService;
            _managerRepo = managerRepo;
            _db = db;
        }

        private string GetUserId()
        {
            var httpContextUser = _userManager.GetUserId(HttpContext.User);
            return httpContextUser;
        }

        public async Task<IActionResult> Index()
        {
            int totalEmployees = _db.Personels.Where(x => x.IsActive == true).Count() + _db.Managers.Where(x => x.IsActive == true).Count();
            ViewBag.TotalEmployees = totalEmployees;
            int totalCompanies = _db.Companies.Where(x => x.IsActive == true).Count();
            ViewBag.TotalCompanies = totalCompanies;
            int totalPassiveEmployees = _db.Personels.Where(x => x.IsActive == false).Count() + _db.Managers.Where(x => x.IsActive == false).Count();
            ViewBag.TotalPassiveEmployees = totalPassiveEmployees;
            decimal sumSalaryEmployees = _db.Personels.Where(x => x.IsActive == true).Sum(x => x.Maas) + _db.Managers.Where(x => x.IsActive == true).Sum(x => x.Maas);
            ViewBag.SumSalaryEmployees = sumSalaryEmployees;
            var vm = await _adminViewModelService.GetAdminSummaryViewModelAsync(_adminViewModelService.GetActiveAdminId(GetUserId()));
            return View(vm);
        }

        public async Task<IActionResult> AdminDetails()
        {
            var vm = await _adminViewModelService.GetAdminViewModelAsync(_adminViewModelService.GetActiveAdminId(GetUserId()));
            return View(vm);
        }

        public async Task<IActionResult> AdminUpdate()
        {
            var vm = await _adminViewModelService.GetAdminViewModelAsync(_adminViewModelService.GetActiveAdminId(GetUserId()));
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminUpdate(AdminViewModel adminViewModel)
        {
            if (ModelState["Address"].Errors.Count == 0 && ModelState["PhoneNumber"].Errors.Count == 0)
            {
                await _adminViewModelService.UpdateAdminAsync(adminViewModel, _adminViewModelService.GetActiveAdminId(GetUserId()));
                return RedirectToAction("Index", "Admin", adminViewModel);
            }
            return View();
        }

        public async Task<IActionResult> ListCompany()
        {
            var companies = await _adminViewModelService.ListCompanyAsync();
            return View(companies);
        }

        private void GetAllToShow()
        {
            var departments = _personelViewModelService.BringDepartments();
            List<SelectListItem> departmentList = new List<SelectListItem>();
            foreach (var department in departments)
            {
                departmentList.Add(new SelectListItem
                {
                    Text = department.Name,
                    Value = department.Id.ToString()
                });
            }

            ViewBag.DepartmentList = departmentList;

            ViewBag.Managers = _personelViewModelService.BringManagers();
            List<SelectListItem> managerList = new List<SelectListItem>();
            foreach (var manager in ViewBag.Managers)
            {
                managerList.Add(new SelectListItem
                {
                    Text = manager.FirstName + " " + manager.Surname,
                    Value = manager.Id.ToString()
                });
            }

            ViewBag.ManagerList = managerList;

            ViewBag.Companies = _personelViewModelService.BringCompanies();
            List<SelectListItem> companyList = new List<SelectListItem>();
            foreach (var company in ViewBag.Companies)
            {
                companyList.Add(new SelectListItem
                {
                    Text = company.Name,
                    Value = company.Id.ToString()
                });
            }

            ViewBag.CompanyList = companyList;

            ViewBag.Packages = _personelViewModelService.BringPackages();
            List<SelectListItem> packageList = new List<SelectListItem>();
            foreach (var package in ViewBag.Packages)
            {
                packageList.Add(new SelectListItem
                {
                    Text = package.Name,
                    Value = package.Id.ToString()
                });
            }

            ViewBag.PackageList = packageList;
        }

        public IActionResult AddCompany()
        {
            GetAllToShow();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompany(CompanyViewModel companyViewModel)
        {

            if (ModelState.IsValid)
            {
                await _adminViewModelService.AddCompanyViewModelAsync(companyViewModel);
                return RedirectToAction("ListCompany");
            }
            GetAllToShow();
            return View("AddCompany");
        }

        public async Task<IActionResult> UpdateCompany(int id)
        {
            ViewBag.Packages = _personelViewModelService.BringPackages();
            List<SelectListItem> packageList = new List<SelectListItem>();
            foreach (var package in ViewBag.Packages)
            {
                packageList.Add(new SelectListItem
                {
                    Text = package.Name,
                    Value = package.Id.ToString()
                });
            }

            ViewBag.PackageListe = packageList;
            var vm = await _adminViewModelService.GetCompanyViewModelAsync(id);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCompany(CompanyViewModel companyViewModel, int id)
        {
            if (ModelState["Address"].Errors.Count == 0 && ModelState["PhoneNumber"].Errors.Count == 0)
            {
                await _adminViewModelService.UpdateCompanyAsync(companyViewModel, id);
                return RedirectToAction("ListCompany", "Admin", companyViewModel);
            }
            return View();
        }

        public async Task<IActionResult> CompanyDetails(int id, List<Manager> managers)
        {
            var vm = await _adminViewModelService.GetCompanyViewModelAsync(id);
            managers = _managerViewModelService.GetAllManagers();
            var model = new Tuple<CompanyViewModel, List<Manager>>(vm, managers);
            return View(model);
        }

        public IActionResult AddManager()
        {
            GetAllToShow();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddManager(ManagerViewModel managerViewModel)
        {
            if (ModelState.IsValid)
            {
                await _adminViewModelService.AddManagerViewModelAsync(managerViewModel);
                var subject = "Merhabalar Şirketimize Hoşgeldiniz";
                var body = $"Merhaba {managerViewModel.FirstName} şirket içi işlemlerde kullanabilmen için uygulamamıza kayıt olman gerekli bu sebeple kayıt olurken kullanacağınız mail adresi:{managerViewModel.MailAdress} </br> Üye olduktan sonra {managerViewModel.MailAdress.Split("@")[0]} kullanıcı adınız olarak atanacaktır ve giriş yaparken kullanıcı adınızla giriş yapmak zorundasınız. Üye olmak için </br> <a href=\"https://maybesoft.azurewebsites.net/Identity/Account/Register?returnUrl=%2F\">Üye Ol</a> Linkine tıklayabilirsiniz. İyi çalışmalar Dilerim.";
                var client = new SmtpClient() { Host = "smtp.gmail.com", Port = 587 };
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("maybesoft2023@gmail.com", "ercidunmioyaywpq");
                MailMessage message = new MailMessage("maybesoft2023@gmail.com", managerViewModel.MailAdress, subject, body);
                message.IsBodyHtml = true;
                await client.SendMailAsync(message);
                return RedirectToAction("Index");
            }
            GetAllToShow();
            return View();
        }

        public IActionResult AddPackage()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPackage(PackageViewModel packageViewModel)
        {
            if (ModelState.IsValid)
            {
                await _adminViewModelService.AddPackageViewModelAsync(packageViewModel);
                return RedirectToAction("ListPackages");
            }
            return View();
        }

        public async Task<IActionResult> ListPackages()
        {
            var packages = await _adminViewModelService.ListPackagesAsync();
            return View(packages);
        }

        public IActionResult GetCompaniesByPackageId(int id)
        {
            var companies = _db.Companies.Where(x => x.PackageId == id).Select(x => x.Name);
            return Json(companies);
        }

    }
}
