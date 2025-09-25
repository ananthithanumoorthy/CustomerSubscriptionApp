using System.ComponentModel.DataAnnotations;
namespace CustomerSubscriptionApp.Web.Models
{
    public class UserMaster
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; } = null!;

        [Required, MaxLength(200)]
        public string PasswordHash { get; set; } = null!; // hashed

        [MaxLength(200)]
        public string? Email { get; set; }

        [Required]
        public string Role { get; set; } = "User"; // "Admin" / "User"

        public int Status { get; set; } = 1; // 1 active, 0 deleted etc.
    }
}
