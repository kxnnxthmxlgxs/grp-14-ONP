
using NMBHLabSys.LabManagerSubsystem;
using Microsoft.AspNetCore.Authentication.Cookies;
using NMBDHL.Auth.Repository;
using NMBDHL.DataAccess.SQLDataBaseAccess;
using NMBHDL.LabTechnician;
using NMBDHL.Doctor.Repository;
using NMBDHL.Patient.Repository;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<
    WebApplication2.Services.LabTechnicianDataService>();

builder.Services.AddLabTechnicianSubsystem(
    builder.Configuration);

// Lab Manager subsystem
builder.Services.AddLabManagerSubsystem(
    builder.Configuration);

// Database access
builder.Services.AddScoped<
    ISQLDataBaseAccess,
    SQLDataBaseAccess>();

// Authentication repository
builder.Services.AddScoped<
    IAuthRepository,
    AuthRepository>();

builder.Services.AddScoped<
    IDoctorRepository,
    DoctorRepository>();

builder.Services.AddScoped<
    IPatientRepository,
    PatientRepository>();

// Session support
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cookie authentication
builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/LoginPage";
        options.AccessDeniedPath = "/Home/LoginPage";

        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;

        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

// First-login password enforcement
//app.Use(async (context, next) =>
//{
//    if (context.User.Identity?.IsAuthenticated == true)
//    {
//        var isFirstLogin =
//            context.User.FindFirst("IsFirstLogin")?.Value;

//        if (string.Equals(
//            isFirstLogin,
//            "true",
//            StringComparison.OrdinalIgnoreCase))
//        {
//            var path = context.Request.Path;

//            bool allowed =
//                path.StartsWithSegments(
//                    "/Home/FirstLoginPassword") ||

//                path.StartsWithSegments(
//                    "/Home/ChangeFirstLoginPassword") ||

//                path.StartsWithSegments(
//                    "/Home/LoginPage") ||

//                path.StartsWithSegments(
//                    "/Home/Login") ||

//                path.StartsWithSegments(
//                    "/Home/Logout") ||

//                path.StartsWithSegments("/css") ||
//                path.StartsWithSegments("/js") ||
//                path.StartsWithSegments("/lib") ||
//                path.StartsWithSegments("/images") ||

//                path.StartsWithSegments(
//                    "/favicon.ico");

//            if (!allowed)
//            {
//                context.Response.Redirect(
//                    "/Home/FirstLoginPassword");

//                return;
//            }
//        }
//    }

//    await next();
//});

app.UseAuthorization();

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
