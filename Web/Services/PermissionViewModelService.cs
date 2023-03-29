using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Web.Interfaces;
using Web.Models;

namespace Web.Services
{
    public class PermissionViewModelService : IPermissionViewModelService
    {
        private readonly ApplicationDbContext _db;
        private readonly IRepository<Permission> _permissionRepo;
        private readonly IRepository<Personel> _personelRepo;
        private readonly IPersonelViewModelService _personelViewModelService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public PermissionViewModelService(ApplicationDbContext db, IRepository<Permission> permissionRepo, IRepository<Personel> personelRepo, IPersonelViewModelService personelViewModelService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _db = db;
            _permissionRepo = permissionRepo;
            _personelRepo = personelRepo;
            _personelViewModelService = personelViewModelService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _env = env;
        }

        public async Task DeletePermission(int id)
        {
            var permission = await _permissionRepo.GetByIdAsync(id);
            await _permissionRepo.DeleteAsync(permission);
        }

        public Task FindPersonel(int personelId)
        {
            var personel = _permissionRepo.FirstOrDefaultAsync(personelId);
            return personel;
        }

        public async Task<PermissionViewModel> GetByIdAsync(int id)
        {
            return await _db.FindAsync<PermissionViewModel>(id);
        }

        public  Task<List<PermissionViewModel>> GetPersonelPermissions(int personelId)
        {
            return (Task<List<PermissionViewModel>>)_db.Permissions.ToList().Where(x => x.Id == personelId);
        }

        public async Task PermissionRequestAsync(PermissionViewModel vm)
        {
            await _permissionRepo.AddAsync(ViewModelToPermission(vm));
        }

        public async Task<PermissionViewModel> UpdatePermissionAsync(PermissionViewModel permissionViewModel)
        {
            var permission = await _permissionRepo.GetByIdAsync(permissionViewModel.Id);
            permission.PermissionFile = permissionViewModel.PermissionFileUrl;
            await _permissionRepo.UpdateAsync(permission);
            return permissionViewModel;
        }

        private string GetUserId()
        {
            var httpContextUser = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            return httpContextUser;
        }

        public Permission ViewModelToPermission(PermissionViewModel permissionViewModel)
        {
            
            var permissions = new Permission
            {
                Id = permissionViewModel.Id,
                PermissionType = permissionViewModel.PermissionType,
                CountOfPermittedDays = permissionViewModel.CountOfPermittedDays,
                StartOfPermissionDate = permissionViewModel.StartOfPermissionDate,
                EndOfPermissionDate = permissionViewModel.EndOfPermissionDate,
                RequestDate = (DateTime)permissionViewModel.RequestDate,
                ReplyDate = permissionViewModel.ReplyDate,
                ApprovalState= permissionViewModel.ApprovalState,
                
            };

            var personelId = _db.Personels.FirstOrDefault(x => x.AppUserId == GetUserId()).Id;
            permissions.PersonelId = personelId;
            var personelReal = _db.Personels.Include(p => p.Permissions).FirstOrDefault(p => p.Id == permissionViewModel.PersonelId);
            permissions.Personel = personelReal;

            if (permissions.PermissionFile != null)
            {
                permissions.PermissionFile = YukleAsync(permissionViewModel.PermissionFile);
            }

            return permissions;
        }

        private string YukleAsync(IFormFile? dosya)
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
    }
}
