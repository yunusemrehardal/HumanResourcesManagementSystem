using ApplicationCore.Entities;
using Web.Models;

namespace Web.Interfaces
{
    public interface IAdminViewModelService
    {
        Task<AdminViewModel> GetAdminViewModelAsync(int adminId);
        Task<AdminViewModel> UpdateAdminAsync(AdminViewModel adminViewModel, int adminId);
        Task<CompanyViewModel> UpdateCompanyAsync(CompanyViewModel companyViewModel, int id);
        Task AddCompanyViewModelAsync(CompanyViewModel companyViewModel);
        Task<AdminSummaryViewModel> GetAdminSummaryViewModelAsync(int adminId);
        int GetActiveAdminId(string adminId);
        Task<List<Company>> ListCompanyAsync();
        Task<List<Package>> ListPackagesAsync();
        Task<CompanyViewModel> GetCompanyViewModelAsync(int id);
        Task AddManagerViewModelAsync(ManagerViewModel managerViewModel);

        Task AddPackageViewModelAsync(PackageViewModel packageViewModel);
    }
}
