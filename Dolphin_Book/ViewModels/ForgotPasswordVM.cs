using System.ComponentModel.DataAnnotations;

namespace Dolphin_Book.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required, MaxLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string? Id { get; set; }
        public string? Token { get; set; }
    }
}
