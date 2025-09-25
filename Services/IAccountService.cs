using CustomerSubscriptionApp.Web.Models;

namespace CustomerSubscriptionApp.Web.Services
{
    public interface IAccountService
    {
        Task<UserMaster?> FindByUsernameAsync(string username);
        Task<UserMaster?> FindByUserIdAsync(int userId);
        Task CreateAsync(UserMaster user);
        Task UpdatePasswordHashAsync(int userId, string passwordHash);
    }
}
