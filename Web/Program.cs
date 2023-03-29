using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Tls;
using System.Text.Json;
using Web.Interfaces;
using Web.SerializingClass;
using Web.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AppIdentityDbContext");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
builder.Services.AddScoped<IManagerViewModelService, ManagerViewModelService>();
builder.Services.AddScoped<IPersonelViewModelService, PersoneViewModelService>();
builder.Services.AddScoped<IAdminViewModelService, AdminViewModelService>();
builder.Services.AddScoped<IAdvanceViewModelService, AdvanceViewModelService>();
builder.Services.AddScoped<IPermissionViewModelService, PermissionViewModelService>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>(); 


builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    IgnoreNullValues = true
});

builder.Services.AddSingleton<ITempDataSerializer, JsonTempDataSerializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Personel",
        pattern: "Personel",
        defaults: new { controller = "Personel", action = "Index" },
        constraints: new { roles = "Personel" });

    endpoints.MapControllerRoute(
        name: "Manager",
        pattern: "Manager",
        defaults: new { controller = "Manager", action = "Index" },
        constraints: new { roles = "Manager" });

    //endpoints.MapControllerRoute(
    //     name: "default",
    //     pattern: "{controller}/{action}/{id?}",
    //     defaults: new { controller = "Identity", action = "Account/Login" });
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await ApplicationDbContextSeed.SeedAsync(roleManager, userManager);
}

app.Run();
