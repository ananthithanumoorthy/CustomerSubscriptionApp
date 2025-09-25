using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using CustomerSubscriptionApp.Web.Models;
using CustomerSubscriptionApp.Web.ViewModels;
using CustomerSubscriptionApp.Web.Services;
using CustomerSubscriptionApp.Web.Repositories;
namespace CustomerSubscriptionApp.Web.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAuthUserservice _svc;
        private readonly IEmailSender _email;

        public AccountController(IAuthUserservice svc, IEmailSender email)
        {
            _svc = svc;
            _email = email;
        }

        [HttpGet("Login")]
        public IActionResult Login() => View();

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel vm)
        {
            var user = await _svc.AuthenticateAsync(vm.UserName, vm.Password);
            if (user == null) { ModelState.AddModelError("", "Invalid credentials"); return View(vm); }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "CustomerSubscription");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword() => View();

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordViewModel vm)
        {
            var token = await _svc.GeneratePasswordResetTokenAsync(vm.UserName);
            if (token == null) { ModelState.AddModelError("", "User not found"); return View(vm); }

            var resetLink = Url.Action("ResetPassword", "Account", new { token }, Request.Scheme);
            await _email.SendEmailAsync(vm.UserName, "Reset password", $"Use link: {resetLink}");
            ViewBag.Message = "Reset link sent (dev: console).";
            return View();
        }

        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(string token) { return View(new ResetPasswordViewModel { Token = token }); }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel vm)
        {
            var ok = await _svc.ResetPasswordAsync(vm.Token!, vm.NewPassword!);
            if (!ok) { ModelState.AddModelError("", "Invalid or expired token"); return View(vm); }
            ViewBag.Message = "Password reset successful";
            return View();
        }
    }
}
