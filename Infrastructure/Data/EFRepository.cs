using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EFRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _db; 

        public EFRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<T> AddAsync(T entity)
        {
            _db.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
       
        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await _db.Set<T>().WithSpecification(specification).CountAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<T> FirstAsync(ISpecification<T> specification)
        {
            return await _db.Set<T>().WithSpecification(specification).FirstAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification)
        {
            return await _db.Set<T>().WithSpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(int id)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(ISpecification<T> specification)
        {
            return await _db.Set<T>().ToListAsync(specification);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _db.FindAsync<T>(id);
        }

        public List<Department> GetDepartments()
        {
            return _db.Departments.ToList();
        }

        public void ManagerToCompanyInclude()
        {
            _db.Managers.Include(x => x.Company);
        }

        public void ManagerToDepartmentInclude()
        {
            _db.Set<Manager>().Include(x => x.Departments).ToList();
        }

        public void PersonelToAllInclude()
        {
            _db.Set<Personel>().Include(x => x.Department);
            _db.Set<Personel>().Include(x => x.Manager);
            _db.Set<Personel>().Include(x => x.Company);

            _db.Set<Manager>().Include(x => x.Departments).ToList();
            _db.Set<Manager>().Include(x => x.Personels).ToList();
            _db.Set<Manager>().Include(x => x.Company);

            _db.Set<Company>().Include(x => x.Departments).ToList();
            _db.Set<Company>().Include(x => x.Managers).ToList();
            _db.Set<Company>().Include(x => x.Personels).ToList();

            _db.Set<Department>().Include(x => x.Company);
            _db.Set<Department>().Include(x => x.Manager);
            _db.Set<Department>().Include(x => x.Personels).ToList();
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
