using CustomerSubscriptionApp.Web.Data;
using CustomerSubscriptionApp.Web.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using CustomerSubscriptionApp.Web.Services;
namespace CustomerSubscriptionApp.Web.Repositories
{
    public class AccountRepository:IAccountService
    {

        private readonly AppDbContext _context;
        private readonly IDataProtector _protector;
        private readonly IConfiguration _configuration;

        public AccountRepository(AppDbContext context, IDataProtectionProvider provider, IConfiguration configuration)
        {
            _context = context;
            _protector = provider.CreateProtector("PasswordResetToken");
            _configuration = configuration;
        }

        public async Task<UserMaster?> FindByUsernameAsync(string username)
        {
            return await _context.UserMasters.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == username && u.Status == 1);
        }

        public async Task<UserMaster?> FindByUserIdAsync(int userId)
        {
            return await _context.UserMasters.FindAsync(userId);
        }

        public async Task CreateAsync(UserMaster user)
        {
            _context.UserMasters.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePasswordHashAsync(int userId, string passwordHash)
        {
            var user = await _context.UserMasters.FindAsync(userId);
            if (user == null) throw new InvalidOperationException("User not found");
            user.PasswordHash = passwordHash;
            await _context.SaveChangesAsync();
        }

        // Helper to generate/validate tokens (used by service)
        public string ProtectToken(string value, TimeSpan validFor)
        {
            var payload = $"{value}|{DateTime.UtcNow.Add(validFor).Ticks}";
            return _protector.Protect(payload);
        }

        public (bool ok, string value) UnprotectToken(string token)
        {
            try
            {
                var un = _protector.Unprotect(token);
                var parts = un.Split('|');
                if (parts.Length != 2) return (false, string.Empty);

                var value = parts[0];
                var ttl = new DateTime(long.Parse(parts[1]));
                if (DateTime.UtcNow > ttl) return (false, string.Empty);
                return (true, value);
            }
            catch
            {
                return (false, string.Empty);
            }
        }
    }
}

