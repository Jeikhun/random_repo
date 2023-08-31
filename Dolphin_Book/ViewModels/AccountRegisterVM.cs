using System.ComponentModel.DataAnnotations;

namespace Dolphin_Book.ViewModels
{
    public class AccountRegisterVM
    {
        [Required(ErrorMessage = "Ümumi ad daxil edilməlidir")]
        [MaxLength(50, ErrorMessage = "Ümumi ad maksimum 50 simvol olmalıdır")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir")]
        [MaxLength(50, ErrorMessage = "İstifadəçi adı maksimum 50 simvol olmalıdır")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Mail düzgün formatda deyil")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifrə daxil edilməlidir")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Şifrə və şifrə təsdiqi bir-biri ilə uyğun deyil")]
        public string ConfirmPassword { get; set; }
    }

}
