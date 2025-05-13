using Potato.Validators;
using System.ComponentModel.DataAnnotations;

namespace Potato.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите фамилию")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }


        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string EmailReg { get; set; }

        [Required]
        [Display(Name = "Дата рождения")]
        [BirthDateValidation]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; } = new DateTime(2000, 6, 15);


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(50, ErrorMessage = "Поле \"{0}\" должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string PasswordReg { get; set; }

        [Required]
        [Compare("PasswordReg", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        [Required]
        [Display(Name = "Логин")]
        [StringLength(15, ErrorMessage = "Поле \"{0}\" должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 4)]
        public string UserName { get; set; }
    }
}
