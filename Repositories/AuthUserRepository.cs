using CustomerSubscriptionApp.Web.Models;
using CustomerSubscriptionApp.Web.Services;
using Microsoft.AspNetCore.Identity;

namespace CustomerSubscriptionApp.Web.Repositories
{
    public class AuthUserRepository :IAuthUserservice
    {
        private readonly IAccountService _repo;
        private readonly PasswordHasher<UserMaster> _hasher;
        private readonly IConfiguration _config;

        public AuthUserRepository(IAccountService repo, IConfiguration config)
        {
            _repo = repo;
            _hasher = new PasswordHasher<UserMaster>();
            _config = config;
        }

        public async Task<UserMaster?> AuthenticateAsync(string username, string password)
        {
            var user = await _repo.FindByUsernameAsync(username);
            if (user == null) return null;
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<bool> CreateUserAsync(string username, string email, string password, string role = "User")
        {
            var exists = await _repo.FindByUsernameAsync(username);
            if (exists != null) return false;
            var user = new UserMaster { UserName = username, Email = email, Role = role };
            user.PasswordHash = _hasher.HashPassword(user, password);
            await _repo.CreateAsync(user);
            return true;
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(string username)
        {
            var user = await _repo.FindByUsernameAsync(username);
            if (user == null) return null;

            var ttlMinutes = _config.GetValue("PasswordReset:TokenMinutes", 30);
            // create token as "userId" and expiry ticks via repository protector
            var protectorMethod = typeof(Repositories.AccountRepository).GetMethod("ProtectToken", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            // We can't call internal method via interface, so cast:
            if (_repo is Repositories.AccountRepository ar)
            {
                var token = ar.ProtectToken(user.UserId.ToString(), TimeSpan.FromMinutes(ttlMinutes));
                return token;
            }
            return null;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            if (_repo is Repositories.AccountRepository ar)
            {
                var res = ar.UnprotectToken(token);
                if (!res.ok) return false;
                if (!int.TryParse(res.value, out var userId)) return false;
                var user = await _repo.FindByUserIdAsync(userId);
                if (user == null) return false;
                var hash = _hasher.HashPassword(user, newPassword);
                await _repo.UpdatePasswordHashAsync(userId, hash);
                return true;
            }
            return false;
        }

        public Task<UserMaster?> FindByUsernameAsync(string username) => _repo.FindByUsernameAsync(username);
    }
}
