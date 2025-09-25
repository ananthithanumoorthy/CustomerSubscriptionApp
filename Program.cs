using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using CustomerSubscriptionApp.Web.Data;
using CustomerSubscriptionApp.Web.Repositories;
using CustomerSubscriptionApp.Web.Services;

// Ensure the Microsoft.EntityFrameworkCore.Sqlite package is installed
// Add the following using directive to resolve the 'UseSqlite' method
using Microsoft.EntityFrameworkCore.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DB - default SQLite (appsettings.json has connection)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication - cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { options.LoginPath = "/Account/Login"; });

// DI - repositories & services
builder.Services.AddScoped<IAuthUserservice, AuthUserRepository>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionRepository>();
builder.Services.AddScoped<IAccountService, AccountRepository>();
builder.Services.AddSingleton<IEmailSender, ConsoleEmailSender>();

// data protection available automatically
builder.Services.AddDataProtection();

builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://127.0.0.1:5000");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

