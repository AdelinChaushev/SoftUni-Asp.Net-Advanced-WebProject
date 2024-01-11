using JobFinder.Core.Contracts;
using JobFinder.Core.Services;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<JobFinderDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<IJobListingServiceInterface, JobListingService>();
builder.Services.AddScoped<IInterviewServiceInterface, InterviewService>();
builder.Services.AddScoped<IPictureServiceInterface, PictureService>();
builder.Services.AddScoped<IResumeServiceInterface, ResumeService>();
builder.Services.AddScoped<ICompanyServiceInterface, CompanyService>();
builder.Services.AddScoped<IUserServiceInterface, UserService>();


builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;

    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = false;
})
    
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<JobFinderDbContext>();
builder.Services.ConfigureApplicationCookie(c =>
{
    c.Cookie.HttpOnly = true;
    c.LoginPath = "/Account/Login";
});



builder.Services.AddControllersWithViews();


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
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Administration",
    pattern: "Administration/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "Employer",
    areaName: "Employer",
    pattern: "Employer/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();



app.Run();
