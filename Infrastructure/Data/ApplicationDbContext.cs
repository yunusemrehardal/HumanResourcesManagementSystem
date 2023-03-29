using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<Manager> Managers => Set<Manager>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Personel> Personels => Set<Personel>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Advance> Advances => Set<Advance>();
        public DbSet<Package> Packages => Set<Package>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            ApplicationDbContextSeed.SeedIdentity(builder);
            ApplicationDbContextSeed.DataSeedAsync(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
