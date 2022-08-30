using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EmployeeManagement.Security;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IUnitOfWorkDal, UnitOfWorkDal>();

builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CantDeleteHimSelfHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();


builder.Services.AddDbContext<EmpContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    opt.SignIn.RequireConfirmedEmail = true;
    opt.Lockout.MaxFailedAccessAttempts = 3;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(5);
}).
AddEntityFrameworkStores<EmpContext>().AddDefaultTokenProviders();


builder.Services.AddAuthentication().
AddGoogle(opt =>
{
    opt.ClientSecret = "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";
    opt.ClientId = "900391360936-mvvpksra6m7k69m1t44eh16vo0vfjume.apps.googleusercontent.com";
}).
AddFacebook(opt =>
{
    opt.AppId = "411367324421122";
    opt.AppSecret = "7de0c7b6503b1b2e99061815633d8325";
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
    opt.TokenLifespan = TimeSpan.FromHours(48);
    
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
    opt.AddPolicy("DeleteRolePolicy", opt => opt.RequireClaim("Delete Role", "True").RequireRole("Admin").AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
    opt.AddPolicy("CreateRolePolicy", opt => opt.RequireClaim("Create Role", "True").RequireRole("Admin"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithReExecute("/Error/{0}");


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{Controller=Employee}/{Action=EmployeeList}/{id?}");

app.Run();
