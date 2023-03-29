using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Interfaces;
using Web.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Web.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;

namespace Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly IManagerViewModelService _managerViewModelService;
        private readonly IWebHostEnvironment _env;
        private readonly IPersonelViewModelService _personelViewModelService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IAdvanceViewModelService _advanceViewModelService;
        private readonly IRepository<Advance> _advanceRepo;
        private readonly IRepository<Personel> _personelRepo;
        private readonly IRepository<Permission> _permissionRepo;
        private readonly IPermissionViewModelService _permissionViewModelService;
        private readonly IEmailSender _service;

        public ManagerController(IManagerViewModelService managerViewModelService, IWebHostEnvironment env, IPersonelViewModelService personelViewModelService, IEmailSender service, UserManager<ApplicationUser> userManager, ApplicationDbContext db, IAdvanceViewModelService advanceViewModelService, IRepository<Advance> advanceRepo, IRepository<Personel> personelRepo, IRepository<Permission> permissionRepo, IPermissionViewModelService permissionViewModelService)
        {
            _managerViewModelService = managerViewModelService;
            _env = env;
            _personelViewModelService = personelViewModelService;
            _service = service;
            _userManager = userManager;
            _db = db;
            _advanceViewModelService = advanceViewModelService;
            _advanceRepo = advanceRepo;
            _personelRepo = personelRepo;
            _permissionRepo = permissionRepo;
            _permissionViewModelService = permissionViewModelService;
        }

        private string GetUserId()
        {
            var httpContextUser = _userManager.GetUserId(HttpContext.User);
            return httpContextUser;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _managerViewModelService.GetSummaryManagerViewModelAsync(_managerViewModelService.GetActiveManagerId(GetUserId()));
            return View(vm);
        }

        public async Task<IActionResult> ManagerDetails()
        {
            var vm = await _managerViewModelService.GetManagerViewModelAsync(_managerViewModelService.GetActiveManagerId(GetUserId()));
            return View(vm);
        }

        public async Task<IActionResult> ManagerUpdate()
        {
            var vm = await _managerViewModelService.GetManagerViewModelAsync(_managerViewModelService.GetActiveManagerId(GetUserId()));
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagerUpdate(ManagerViewModel managerViewModel)
        {
            if (ModelState["Address"].Errors.Count == 0 && ModelState["PhoneNumber"].Errors.Count == 0)
            {
                await _managerViewModelService.UpdateManagerAsync(managerViewModel, _managerViewModelService.GetActiveManagerId(GetUserId()));
                return RedirectToAction("Index", "Manager", managerViewModel);
            }
            return View();
        }

        public IActionResult AddPersonel()
        {
            GetAllToShow();
            return View();
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
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPersonel(PersonelViewModel personelViewModel)
        {
            if (ModelState.IsValid)
            {
                var subject = "Merhabalar Şirketimize Hoşgeldiniz";
                var body = $"Merhaba {personelViewModel.FirstName} şirket içi işlemlerde kullanabilmen için uygulamamıza kayıt olman gerekli bu sebeple kayıt olurken kullanacağınız mail adresi:{personelViewModel.MailAdress} </br> Üye olduktan sonra {personelViewModel.MailAdress.Split("@")[0]} kullanıcı adınız olarak atanacaktır ve giriş yaparken kullanıcı adınızla giriş yapmak zorundasınız. Üye olmak için </br> <a href=\"https://maybesoft.azurewebsites.net/Identity/Account/Register?returnUrl=%2F\">Üye Ol</a> Linkine tıklayabilirsiniz. İyi çalışmalar Dilerim.";
                var client = new SmtpClient() { Host = "smtp.gmail.com", Port = 587 };
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("maybesoft2023@gmail.com", "ercidunmioyaywpq");
                MailMessage message = new MailMessage("maybesoft2023@gmail.com", personelViewModel.MailAdress, subject, body);
                message.IsBodyHtml = true;
                await client.SendMailAsync(message);

                await _managerViewModelService.AddPersonelViewModelAsync(personelViewModel);
                return RedirectToAction("Index");

            }



            GetAllToShow();
            return View("AddPersonel");
        }

        public async Task<IActionResult> ListPersonel()
        {
            var personels = await _personelViewModelService.ListPersonelAsync();

            return View(personels);
        }

        public async Task<IActionResult> PersonelDetails(int id)
        {

            var personels = await _personelViewModelService.GetPersonelViewModelAsync(id);
            TempData["id"] = personels.Id;
            return View(personels);
        }

        public IActionResult ShowAllAdvanceRequests()
        {
            List<AdvanceViewModel> advanceViewModelList = AdvanceViewModelList();
            return View(advanceViewModelList);
        }

        public IActionResult ShowAllPermissionRequests()
        {
            List<PermissionViewModel> permissionViewModelList = PermissionViewModelList();
            return View(permissionViewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> ShowAllAdvanceRequests(int id)
        {
            var advance = await _db.Advances.Include(p => p.Personel).FirstOrDefaultAsync(x => x.Id == id);

            var isActive = Request.Form["isActive_" + id] == "true";
            var isItConfirmed = Request.Form["isItConfirmed_" + id] == "true";

            if (advance != null)
            {
                List<AdvanceViewModel> advanceViewModelList = AdvanceViewModelList();

                advance.IsActive = isActive;
                advance.IsItConfirmed = isItConfirmed;
                advance.AdvanceApprovalDate = DateTime.UtcNow;

                if ((isActive && isItConfirmed) && (advance.AdvancePaymentAccomodation == null && advance.AdvancePaymentFood == null && advance.AdvancePaymentWay == null && advance.AdvancePaymentOther == null))
                {
                    decimal totalAdvanceRequested = _db.Advances.Where(x => x.PersonelId == advance.Personel.Id).Sum(x => x.AdvancePaymentRequest);
                    decimal remainingAdvanceLimit = advance.Personel.MaxAdvanceLimit - totalAdvanceRequested;

                    if (remainingAdvanceLimit >= 0)
                    {
                        advance.RemainingAdvancePaymentRequest = remainingAdvanceLimit;
                    }
                    else
                    {

                        ViewBag.ErrorMessage = "Personelin avans limiti dolmuş. İstek onaylanamaz.";
                        return View(advanceViewModelList);


                    }

                    if (advance.RemainingAdvancePaymentRequest < 0)
                    {

                        ViewBag.ErrorMessage = "Personelin avans limiti dolmuş. İstek onaylanamaz.";
                        return View(advanceViewModelList);
                    }
                }
                else if (isActive && isItConfirmed)
                {
                    advance.AdvanceApprovalDate = DateTime.UtcNow;
                    decimal totalAdvanceRequested = _db.Advances.Where(x => (x.PersonelId == advance.Personel.Id) && x.AdvancePaymentAccomodation == null && x.AdvancePaymentFood == null && x.AdvancePaymentOther == null && x.AdvancePaymentWay == null).Sum(x => x.AdvancePaymentRequest);
                    decimal remainingAdvanceLimit = advance.Personel.MaxAdvanceLimit - totalAdvanceRequested;
                    advance.RemainingAdvancePaymentRequest = remainingAdvanceLimit;
                }

                _db.SaveChanges();
            }
            return RedirectToAction("ShowAllAdvanceRequests");
        }

        [HttpPost]
        public async Task<IActionResult> ShowAllPermissionRequests(int id)
        {
            var permission = await _db.Permissions.Include(p => p.Personel).FirstOrDefaultAsync(x => x.Id == id);

            var approvalState = Request.Form["approvalState_" + id] == "true";

            if (permission != null)
            {
                List<PermissionViewModel> permissionViewModelList = PermissionViewModelList();

                permission.ApprovalState = approvalState;
                permission.ReplyDate = DateTime.UtcNow;

                _db.SaveChanges();
            }
            return RedirectToAction("ShowAllPermissionRequests");
        }

        private List<AdvanceViewModel> AdvanceViewModelList()
        {
            var list = _db.Advances.ToList();

            List<AdvanceViewModel> advanceViewModelList = list.Select(a => new AdvanceViewModel
            {
                Id = a.Id,
                AdvancePaymentRequest = a.AdvancePaymentRequest,
                AdvanceRequestDate = a.AdvanceRequestDate,
                AdvanceType = a.AdvanceType,
                Description = a.Description,
                IsItConfirmed = a.IsItConfirmed,
                RemainingAdvancePaymentRequest = a.RemainingAdvancePaymentRequest,
                IsActive = a.IsActive,
                PersonelId = a.PersonelId,
                Personel = _db.Personels.Include(p => p.Advances).FirstOrDefault(p => p.Id == a.PersonelId),
                AdvanceFileUrl = a.AdvanceFile,
                Currency = a.Currency,
                AdvanceApprovalDate = a.AdvanceApprovalDate,
                AdvancePaymentFood = a.AdvancePaymentFood,
                AdvancePaymentOther = a.AdvancePaymentOther,
                AdvancePaymentWay = a.AdvancePaymentWay,
                AdvancePaymentAccomodation = a.AdvancePaymentAccomodation,
                TotalAdvancePayment = a.TotalAdvancePayment
            }).ToList();
            return advanceViewModelList;
        }

        private List<PermissionViewModel> PermissionViewModelList()
        {
            var list = _db.Permissions.ToList();

            List<PermissionViewModel> permissionViewModelList = list.Select(a => new PermissionViewModel
            {
                Id = a.Id,
                PermissionType = a.PermissionType,
                StartOfPermissionDate = a.StartOfPermissionDate,
                EndOfPermissionDate = a.EndOfPermissionDate,
                RequestDate = a.RequestDate,
                ApprovalState = a.ApprovalState,
                ReplyDate = a.ReplyDate,
                PersonelId = a.PersonelId,
                Personel = _db.Personels.Include(p => p.Permissions).FirstOrDefault(p => p.Id == a.PersonelId),
                PermissionFileUrl = a.PermissionFile,

            }).ToList();
            return permissionViewModelList;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePersonelSalary(int Id, decimal Maas)
        {
            if (ModelState.IsValid)
            {
                var personel = await _personelViewModelService.GetPersonelAsync(Id);
              
                personel.Maas = Maas;
                await _personelRepo.UpdateAsync(personel);
                return RedirectToAction("Index", "Manager");

            }
            var personels = await _personelViewModelService.GetPersonelViewModelAsync(Id);
            TempData["id"] = personels.Id;
            return View("PersonelDetails", personels);

        }


    }
}
