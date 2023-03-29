using ApplicationCore.Entities;
using Web.Models;

namespace Web.Interfaces
{
    public interface IManagerViewModelService
    {
        Task<ManagerViewModel> GetManagerViewModelAsync(int managerId);
        Task<ManagerViewModel> UpdateManagerAsync(ManagerViewModel managerViewModel, int managerId);
        Task<ManagerSummaryViewModel> GetSummaryManagerViewModelAsync(int managerId);
        Task AddPersonelViewModelAsync(PersonelViewModel personelViewModel);
        int GetActiveManagerId(string managerId);
        List<Manager> GetAllManagers();
    }
}
