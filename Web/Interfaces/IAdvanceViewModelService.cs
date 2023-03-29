using ApplicationCore.Entities;
using Web.Models;

namespace Web.Interfaces
{
    public interface IAdvanceViewModelService
    {
        Task AdvanceRequestAsync(AdvanceViewModel vm);
        Task<List<AdvanceViewModel>> GetPersonelAdvances(int personelId);
        Task FindPersonel(int personelId);
        Task DeleteAdvance(int id);
        Task<AdvanceViewModel> UpdateAdvanceAsync(AdvanceViewModel advanceViewModel);
        Task<AdvanceViewModel> GetByIdAsync(int id);
    }
}
