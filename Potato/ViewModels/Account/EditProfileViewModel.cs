using System.ComponentModel.DataAnnotations;

namespace Potato.ViewModels.Account
{
    public class EditProfileViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Логин")]
        [StringLength(15, ErrorMessage = "Поле \"{0}\" должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 4)]
        public string UserName { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Статус")]
        public string? Status { get; set; }

        [Display(Name = "О себе")]
        public string? About { get; set; }
    }
}
