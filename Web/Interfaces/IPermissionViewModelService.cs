using Web.Models;

namespace Web.Interfaces
{
    public interface IPermissionViewModelService
    {
        Task PermissionRequestAsync(PermissionViewModel vm);
        Task<List<PermissionViewModel>> GetPersonelPermissions(int personelId);
        Task FindPersonel(int personelId);
        Task DeletePermission(int id);
        Task<PermissionViewModel> UpdatePermissionAsync(PermissionViewModel permissionViewModel);
        Task<PermissionViewModel> GetByIdAsync(int id);
    }
}
