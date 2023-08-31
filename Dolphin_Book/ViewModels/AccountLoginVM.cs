using System.ComponentModel.DataAnnotations;

namespace Dolphin_Book.ViewModels
{
    public class AccountLoginVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
