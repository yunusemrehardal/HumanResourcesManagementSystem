using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class ApplicationDbContextSeed
    {
        public static void DataSeedAsync(ModelBuilder builder)
        {
            // Add seed data for Company entity
            builder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Acme Inc.",
                    Address = "123 Main St.",
                    PhoneNumber = "555-1234",
                    MailAdress = "info@acme.com",
                    Logo = null,
                    ContractEndYear = null,
                    ContractStartYear = null,
                    Departments = null,
                    FoundationYear = null,
                    IsActive = true,
                    Managers = null,
                    MersisNo = "000230230120",
                    Personels= null,
                    TaxDepartment="Ostim",
                    TaxNo="02312",
                    Title="Limited"
                    
                });
           
            // Add seed data for Department entity
            builder.Entity<Department>().HasData(
                new Department
                {
                    Id = 1,
                    Name = "Sales",
                    ManagerId = 1,
                    CompanyId = 1
                },
                new Department
                {
                    Id = 2,
                    Name = "Software",
                    ManagerId = 2,
                    CompanyId = 1
                });

            // Add seed data for Manager entity and Admin entity
            builder.Entity<Manager>().HasData(
                new Manager
                {
                    Id = 1,
                    FirstName = "John",
                    Surname = "Doe",
                    BirthDate = new DateTime(1970, 1, 1),
                    TC = "12345678900",
                    StartDateOfWork = new DateTime(2000, 1, 1),
                    Job = "Sales Manager",
                    Address = "456 Oak St.",
                    PhoneNumber = "555-5678",
                    MailAdress = "john.doe@acme.com",
                    IsActive = false,
                    Gender = true,
                    CompanyId = 1,
                    AppUserId = "2eb6b745-c129-4244-a85b-225cd9f61ed2"
                },
                new Manager
                {
                    Id = 2,
                    FirstName = "Erhan",
                    Surname = "Gok",
                    BirthDate = new DateTime(1970, 1, 1),
                    TC = "12345678900",
                    StartDateOfWork = new DateTime(2000, 1, 1),
                    Job = "Software Developer",
                    Address = "456 Oak St.",
                    PhoneNumber = "555-5678",
                    MailAdress = "erhan.gok@acme.com",
                    IsActive = true,
                    Gender = true,
                    CompanyId = 1,
                    AppUserId = "80179962-814b-4ae3-aef3-a94cb0c8d01e",
                });

            builder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    FirstName = "Kenan",
                    Surname = "Işık",
                    BirthDate = new DateTime(1970, 1, 1),
                    TC = "72697096376",
                    Address = "Istanbul",
                    PhoneNumber = "05555555555",
                    MailAdress = "kenan.isik@bilgeadam.com",
                    Gender = true,
                    BirthPlace = "Adana",
                    AppUserId = "0b9ac8cc-9fd5-4a6d-b456-dafbf68ba98b",
                    Companies = null!
                });
        }

        // Add seed data for Roles
        public static void SeedIdentity(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "4e14a851-b637-4e8a-bb66-f7982895fa85",
                    Name = "Personel",
                    NormalizedName = "PERSONEL"
                },
                new IdentityRole()
                {
                    Id = "edd822bf-ebb9-4772-989b-ecc423c705f2",
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new  IdentityRole()
                {
                    Id = "42f10cf4-273d-461a-a952-5eab02a5e30a",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser()
                {
                    Id = "2eb6b745-c129-4244-a85b-225cd9f61ed2",
                    UserName = "john.doe",
                    NormalizedUserName = "JOHN.DOE",
                    Email = "john.doe@acme.com",
                    NormalizedEmail = "JOHN.DOE@ACME.COM",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Ankara1."),
                    SecurityStamp = string.Empty
                },

                new ApplicationUser()
                {
                    Id = "80179962-814b-4ae3-aef3-a94cb0c8d01e",
                    UserName = "erhan.gok",
                    NormalizedUserName = "ERHAN.GOK",
                    Email = "erhan.gok@acme.com",
                    NormalizedEmail = "ERHAN.GOK@ACME.COM",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Ankara1."),
                    SecurityStamp = string.Empty
                },
                new ApplicationUser()
                {
                    Id = "0b9ac8cc-9fd5-4a6d-b456-dafbf68ba98b",
                    UserName = "kenan.isik",
                    NormalizedUserName = "KENAN.ISIK",
                    Email = "kenan.isik@bilgeadam.com",
                    NormalizedEmail = "KENAN.ISIK@BILGEADAM.COM",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Ankara1."),
                    SecurityStamp = string.Empty
                });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "2eb6b745-c129-4244-a85b-225cd9f61ed2",
                    RoleId = "edd822bf-ebb9-4772-989b-ecc423c705f2"
                },
                new IdentityUserRole<string>
                {
                    UserId = "80179962-814b-4ae3-aef3-a94cb0c8d01e",
                    RoleId = "edd822bf-ebb9-4772-989b-ecc423c705f2"
                },
                new IdentityUserRole<string>
                {
                    UserId = "0b9ac8cc-9fd5-4a6d-b456-dafbf68ba98b",
                    RoleId = "42f10cf4-273d-461a-a952-5eab02a5e30a"
                });
        }

        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            var managerUser = await userManager.Users.FirstOrDefaultAsync(x => x.Email == "john.doe@acme.com");
            await userManager.AddToRoleAsync(managerUser, "Manager");

            var managerUser2 = await userManager.Users.FirstOrDefaultAsync(x => x.Email == "erhan.gok@acme.com");
            await userManager.AddToRoleAsync(managerUser2, "Manager");

            var adminUser = await userManager.Users.FirstOrDefaultAsync(x => x.Email == "kenan.isik@bilgeadam.com");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
