using System.ComponentModel.DataAnnotations;

namespace Potato.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Укажите почту.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
