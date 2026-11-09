using System.ComponentModel.DataAnnotations;

namespace NMBDHL.Auth.Model
{
    public class FirstLoginPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "The password must be at least 12 characters.")]
        [MaxLength(128, ErrorMessage = "The password cannot exceed 128 characters.")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(
            nameof(NewPassword),
            ErrorMessage = "The passwords do not match.")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}