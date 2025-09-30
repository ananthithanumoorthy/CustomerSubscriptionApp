using CustomerSubscriptionApp.Web.Data;
using CustomerSubscriptionApp.Web.Repositories;
using CustomerSubscriptionApp.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
// ✅ Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";     // redirect here if not logged in
        options.LogoutPath = "/Account/Logout";   // optional
        options.AccessDeniedPath = "/Account/AccessDenied"; // optional
    });

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Database - SQLite (you can change to SQL Server if needed)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection for Repository & Service
builder.Services.AddScoped<IAccountService, AccountRepository>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionRepository>();
builder.Services.AddScoped<IEmailSender, ConsoleEmailSender>();
builder.Services.AddScoped<IAuthUserservice, AuthUserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ Enable Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();
