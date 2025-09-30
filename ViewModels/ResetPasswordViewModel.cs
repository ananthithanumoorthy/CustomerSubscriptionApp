using System.ComponentModel.DataAnnotations;

namespace CustomerSubscriptionApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;

        public string? Token { get; set; }
    }
}
