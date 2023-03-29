using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using Grpc.Core;
using Infrastructure.Data;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Text;
using Web.Interfaces;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    [Authorize(Roles = "Personel")]
    public class PersonelController : Controller
    {
        private readonly IPersonelViewModelService _personelViewModelService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdvanceViewModelService _advanceViewModelService;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IRepository<Advance> _advanceRepo;
        private readonly IRepository<Permission> _permissionRepo;
        private readonly IPermissionViewModelService _permissionViewModelService;

        public PersonelController(IPersonelViewModelService personelViewModelService, UserManager<ApplicationUser> userManager, IAdvanceViewModelService advanceViewModelService, ApplicationDbContext db, IWebHostEnvironment env, IRepository<Advance> advanceRepo, IRepository<Permission> permissionRepo, IPermissionViewModelService permissionViewModelService)
        {
            _personelViewModelService = personelViewModelService;
            _userManager = userManager;
            _advanceViewModelService = advanceViewModelService;
            _db = db;
            _env = env;
            _advanceRepo = advanceRepo;
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
            var vm = await _personelViewModelService.GetPersonelSummaryViewModelAsync(_personelViewModelService.GetActivePersonelId(GetUserId()));
            return View(vm);
        }

        public async Task<IActionResult> PersonelDetails()
        {
            var personel = await _personelViewModelService.GetPersonelViewModelAsync(_personelViewModelService.GetActivePersonelId(GetUserId()));
            return View(personel);
        }

        public async Task<IActionResult> PersonelUpdate()
        {
            var vm = await _personelViewModelService.GetPersonelViewModelAsync(_personelViewModelService.GetActivePersonelId(GetUserId()));
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonelUpdate(PersonelViewModel personelViewModel)
        {
            if (ModelState["Address"].Errors.Count == 0 && ModelState["PhoneNumber"].Errors.Count == 0)
            {
                await _personelViewModelService.UpdatePersonelAsync(personelViewModel, _personelViewModelService.GetActivePersonelId(GetUserId()));
                return RedirectToAction("Index", "Personel", personelViewModel);
            }
            return View();
        }

        public async Task<IActionResult> ListAdvanceRequests(AdvanceViewModel advanceViewModel, int personelId)
        {
            personelId = _db.Personels.FirstOrDefault(x => x.AppUserId == GetUserId()).Id;
            var list = _db.Advances.Where(x => x.PersonelId == personelId).ToList();
            List<AdvanceViewModel> advanceViewModelList = list.Select(a => new AdvanceViewModel
            {
                Id = a.Id,
                AdvancePaymentRequest = a.AdvancePaymentRequest,
                AdvanceType = a.AdvanceType,
                Description = a.Description,
                IsItConfirmed = a.IsItConfirmed,
                IsActive = a.IsActive,
                PersonelId = a.PersonelId,
                Personel = a.Personel,
                Currency = a.Currency,
                AdvanceRequestDate = a.AdvanceRequestDate,
                RemainingAdvancePaymentRequest = a.RemainingAdvancePaymentRequest,
                AdvancePaymentWay = a.AdvancePaymentWay,
                AdvanceFileUrl = a.AdvanceFile,
                AdvanceApprovalDate = a.AdvanceApprovalDate,
                AdvancePaymentAccomodation = a.AdvancePaymentAccomodation,
                AdvancePaymentFood = a.AdvancePaymentFood,
                AdvancePaymentOther = a.AdvancePaymentOther,
            }).ToList();
            return View(advanceViewModelList);
        }

        [HttpGet]
        public IActionResult AddAdvanceRequest(AdvanceViewModel advanceViewModel, int personelId)
        {

            personelId = _db.Personels.FirstOrDefault(x => x.AppUserId == GetUserId()).Id;
            advanceViewModel.PersonelId = personelId;
            advanceViewModel.Personel = _db.Personels.Include(p => p.Advances).FirstOrDefault(p => p.Id == advanceViewModel.PersonelId);
            return View(advanceViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdvanceRequest(AdvanceViewModel advanceViewModel)
        {
            advanceViewModel.Personel = _db.Personels.Include(p => p.Advances).FirstOrDefault(p => p.Id == advanceViewModel.PersonelId);
            if (ModelState["AdvancePaymentRequest"].Errors.Count == 0 && ModelState["AdvanceType"].Errors.Count == 0 && ModelState["Description"].Errors.Count == 0)
            {
                await _advanceViewModelService.AdvanceRequestAsync(advanceViewModel);
                return RedirectToAction("ListAdvanceRequests");
            }
            return View(advanceViewModel);

        }


        public async Task<IActionResult> DeleteAdvance(int id)
        {
            await _advanceViewModelService.DeleteAdvance(id);

            return RedirectToAction("ListAdvanceRequests");
        }

        //public IActionResult DownloadPDF()
        //{
        //    string fileName = "file.pdf";
        //    string filePath = Path.Combine(_env.ContentRootPath, "~/downloads/file.pdf");
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        //    return File(fileBytes, "application/pdf", fileName);

        //    //<a href="@Url.Action("DownloadPDF", "ControllerName")">PDF İndir</a> bu kod ile view da çağırmaya çalışacağım

        //}
        [HttpGet]
        public IActionResult UploadAdvanceFile(AdvanceViewModel advanceViewModel)
        {
            return View(advanceViewModel);
        }

        [HttpGet]
        public IActionResult UploadPermissionFile(PermissionViewModel permissionViewModel)
        {
            return View(permissionViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAdvanceFile(int id, IFormFile advanceFile)
        {
            var advance = await _db.Advances.Include(p => p.Personel).FirstOrDefaultAsync(x => x.Id == id);
            var personelReal = _db.Personels.Include(p => p.Advances).FirstOrDefault(p => p.Id == advance.PersonelId);

            advance.AdvanceFile = SaveFile(advanceFile);

            var vm = new AdvanceViewModel
            {
                Id = advance.Id,
                AdvancePaymentRequest = advance.AdvancePaymentRequest,
                AdvanceType = advance.AdvanceType,
                IsActive = advance.IsActive,
                IsItConfirmed = advance.IsItConfirmed,
                Personel = personelReal,
                PersonelId = advance.PersonelId,
                Description = advance.Description,
                Currency = advance.Currency,
                AdvanceFileUrl = advance.AdvanceFile,
                AdvanceApprovalDate = advance.AdvanceApprovalDate,
                AdvancePaymentAccomodation = advance.AdvancePaymentAccomodation,
                AdvancePaymentFood = advance.AdvancePaymentFood,
                AdvancePaymentOther = advance.AdvancePaymentOther,
                AdvancePaymentWay = advance.AdvancePaymentWay,
                AdvanceRequestDate = advance.AdvanceRequestDate,
                RemainingAdvancePaymentRequest = advance.RemainingAdvancePaymentRequest,
                TotalAdvancePayment = advance.TotalAdvancePayment
            };

            _db.Update(advance);
            await _db.SaveChangesAsync();
            return RedirectToAction("ListAdvanceRequests");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPermissionFile(int id, IFormFile permissionFile)
        {
            var permission = await _db.Permissions.Include(p => p.Personel).FirstOrDefaultAsync(x => x.Id == id);
            var personelReal = _db.Personels.Include(p => p.Permissions).FirstOrDefault(p => p.Id == permission.PersonelId);

            permission.PermissionFile = SaveFile(permissionFile);

            var vm = new PermissionViewModel
            {
                Id = permission.Id,
                RequestDate = permission.RequestDate,
                ReplyDate = permission.ReplyDate,
                ApprovalState = permission.ApprovalState,
                Personel = personelReal,
                PersonelId = permission.PersonelId,
                PermissionFileUrl = permission.PermissionFile,
                StartOfPermissionDate = permission.StartOfPermissionDate,
                EndOfPermissionDate = permission.EndOfPermissionDate,
                PermissionType = permission.PermissionType
            };

            _db.Update(permission);
            await _db.SaveChangesAsync();
            return RedirectToAction("ListPermissionRequests");
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
                return fileName;
            }
        }

        public AdvanceViewModel MapAdvanceToViewModel(Advance advance)
        {
            var personelReal = _db.Personels.Include(p => p.Advances).FirstOrDefault(p => p.Id == advance.PersonelId);

            var vm = new AdvanceViewModel
            {
                Id = advance.Id,
                AdvancePaymentRequest = advance.AdvancePaymentRequest,
                AdvanceType = advance.AdvanceType,
                IsActive = advance.IsActive,
                IsItConfirmed = advance.IsItConfirmed,
                Personel = personelReal,
                PersonelId = advance.PersonelId,
                Description = advance.Description,
                Currency = advance.Currency,
                AdvanceFileUrl = advance.AdvanceFile,
                AdvanceApprovalDate = advance.AdvanceApprovalDate,
                AdvancePaymentAccomodation = advance.AdvancePaymentAccomodation,
                AdvancePaymentFood = advance.AdvancePaymentFood,
                AdvancePaymentOther = advance.AdvancePaymentOther,
                AdvancePaymentWay = advance.AdvancePaymentWay,
                AdvanceRequestDate = advance.AdvanceRequestDate,
                RemainingAdvancePaymentRequest = advance.RemainingAdvancePaymentRequest,
                TotalAdvancePayment = advance.TotalAdvancePayment
            };
            return vm;
        }

        public async Task<IActionResult> ListPermissionRequests(int personelId)
        {
            personelId = _db.Personels.FirstOrDefault(x => x.AppUserId == GetUserId()).Id;
            var list = _db.Permissions.Where(x => x.PersonelId == personelId).ToList();
            List<PermissionViewModel> permissionViewModelList = list.Select(a => new PermissionViewModel
            {
                Id = a.Id,
                PermissionType = a.PermissionType,
                StartOfPermissionDate = a.StartOfPermissionDate,
                EndOfPermissionDate = a.EndOfPermissionDate,
                RequestDate = a.RequestDate,
                ApprovalState = a.ApprovalState,
                ReplyDate = a.ReplyDate,
                PermissionFileUrl = a.PermissionFile,


            }).ToList();
            return View(permissionViewModelList);
        }

        [HttpGet]
        public IActionResult AddPermissionRequest(PermissionViewModel permissionViewModel, int personelId)
        {

            personelId = _db.Personels.FirstOrDefault(x => x.AppUserId == GetUserId()).Id;
            permissionViewModel.PersonelId = personelId;
            permissionViewModel.Personel = _db.Personels.Include(p => p.Permissions).FirstOrDefault(p => p.Id == permissionViewModel.PersonelId);

            if (permissionViewModel.Personel.Gender == true) // erkek ise
            {
                var selectList = new SelectList(Enum.GetValues(typeof(PermissionType))
                    .Cast<PermissionType>()
                    .Where(e => e != PermissionType.Dogum_İzni) // Dogumizni hariç tutulur
                    .Select(e => new SelectListItem
                    {
                        Text = Enum.GetName(typeof(PermissionType), e) ?? "N/A",
                        Value = Convert.ToInt32(e).ToString(),
                        Selected = e.Equals(permissionViewModel.PermissionType)
                    }));
                ViewData["PermissionList"] = selectList.Items;
            }
            else // kadın ise
            {
                var selectList = new SelectList(Enum.GetValues(typeof(PermissionType))
                    .Cast<PermissionType>()
                    .Where(e => e != PermissionType.Babalık_İzni) // Babalikizni hariç tutulur
                    .Select(e => new SelectListItem
                    {
                        Text = Enum.GetName(typeof(PermissionType), e) ?? "N/A",
                        Value = Convert.ToInt32(e).ToString(),
                        Selected = e.Equals(permissionViewModel.PermissionType)
                    }));
                ViewData["PermissionList"] = selectList.Items;
            }



            return View(permissionViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPermissionRequest(PermissionViewModel permissionViewModel)
        {
            var selectList = new SelectList(Enum.GetValues(typeof(PermissionType)).Cast<PermissionType>().Select(e =>
            new SelectListItem
            {
                Text = Enum.GetName(typeof(PermissionType), e) ?? "N/A",
                Value = Convert.ToInt32(e).ToString(),
                Selected = e.Equals(permissionViewModel.PermissionType)
            }));
            ViewData["PermissionList"] = selectList.Items;
            permissionViewModel.Personel = _db.Personels.Include(p => p.Permissions).FirstOrDefault(p => p.Id == permissionViewModel.PersonelId);
            await _permissionViewModelService.PermissionRequestAsync(permissionViewModel);

            return RedirectToAction("ListPermissionRequests");

        }

        public async Task<IActionResult> DeletePermission(int id)
        {
            await _permissionViewModelService.DeletePermission(id);
            return RedirectToAction("ListPermissionRequests");
        }

        [HttpGet]
        public IActionResult UploadPermission(PermissionViewModel permissionViewModel)
        {
            return View(permissionViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPermission(int id, IFormFile permissionFile)
        {
            var permission = await _db.Permissions.Include(p => p.Personel).FirstOrDefaultAsync(x => x.Id == id);
            var personelReal = _db.Personels.Include(p => p.Permissions).FirstOrDefault(p => p.Id == permission.PersonelId);

            permission.PermissionFile = PermissionSaveFile(permissionFile);

            var vm = new PermissionViewModel
            {
                Id = permission.Id,
                PermissionType = permission.PermissionType,
                StartOfPermissionDate = permission.StartOfPermissionDate,
                EndOfPermissionDate = permission.EndOfPermissionDate,
                RequestDate = permission.RequestDate,
                ApprovalState = permission.ApprovalState,
                ReplyDate = permission.ReplyDate,
                PermissionFileUrl = permission.PermissionFile,
                Personel = personelReal,
                PersonelId = permission.PersonelId
            };

            _db.Update(permission);
            await _db.SaveChangesAsync();
            return RedirectToAction("ListPermissionRequests");
        }

        private string PermissionSaveFile(IFormFile file)
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
                return fileName;
            }
        }

        public PermissionViewModel MapPermissionToViewModel(Permission permission)
        {
            var personelReal = _db.Personels.Include(p => p.Permissions).FirstOrDefault(p => p.Id == permission.PersonelId);

            var vm = new PermissionViewModel
            {
                Id = permission.Id,
                PermissionType = permission.PermissionType,
                StartOfPermissionDate = permission.StartOfPermissionDate,
                EndOfPermissionDate = permission.EndOfPermissionDate,
                RequestDate = permission.RequestDate,
                ApprovalState = permission.ApprovalState,
                ReplyDate = permission.ReplyDate,
                PermissionFileUrl = permission.PermissionFile,
                Personel = personelReal,
                PersonelId = permission.PersonelId
            };
            return vm;
        }
    }

}

