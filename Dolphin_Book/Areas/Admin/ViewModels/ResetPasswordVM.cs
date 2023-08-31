using System.ComponentModel.DataAnnotations;

namespace Dolphin_Book.Areas.Admin.ViewModels
{
    public class ResetPasswordVM
    {
        public string? Id { get; set; }
        [Required]
        public string Token { get; set; }
        [Required, MaxLength(30), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MaxLength(30), DataType(DataType.Password), Display(Name = "Confirm Password"), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required, MaxLength(30), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
