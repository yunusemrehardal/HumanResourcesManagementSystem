using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace ApplicationCore.Interfaces
{
    public interface IPersonelViewModelService
    {
        Task<PersonelViewModel> GetPersonelViewModelAsync(int personelId);
        Task<PersonelSummaryViewModel> GetPersonelSummaryViewModelAsync(int personelId);
        Task<Personel> GetPersonelAsync(int personelId);
        Task<PersonelViewModel> UpdatePersonelAsync(PersonelViewModel personelViewModel, int personelId);
        Task<List<Personel>> ListPersonelAsync();
        List<Department> BringDepartments();
        List<Manager> BringManagers();
        List<Company> BringCompanies();
        List<Package> BringPackages();
        int GetActivePersonelId(string personelId);
     
    }
}
