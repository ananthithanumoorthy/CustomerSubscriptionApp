using CustomerSubscriptionApp.Web.Models;

namespace CustomerSubscriptionApp.Web.Services
{
    public interface IAuthUserservice
    {
        Task<UserMaster?> AuthenticateAsync(string username, string password);
        Task<bool> CreateUserAsync(string username, string email, string password, string role = "User");
        Task<string?> GeneratePasswordResetTokenAsync(string username); // returns token or null
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<UserMaster?> FindByUsernameAsync(string username);
    }
}
