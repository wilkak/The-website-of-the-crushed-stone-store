using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Shebenka.Models.ViewModels.Account
{
    public class LoginUserVM
    {

        [Required(ErrorMessage = "Поле почты должно быть заполнено")]
        [DisplayName("Логин пользователя")]
        public string EmailAdress { get; set; }
        //[StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 6 символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле пароля должно быть заполнено")]

        [DisplayName("Пароль")]
        public string Password { get; set; }
        [DisplayName("Запомнить меня")]
        public bool RememberMe { get; set; }

    }
}