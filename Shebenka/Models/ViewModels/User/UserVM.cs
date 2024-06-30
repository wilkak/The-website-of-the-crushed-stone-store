using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Shebenka.Models.Data;

namespace Shebenka.Models.ViewModels.Account
{
    public class UserVM
    {
        public UserVM()
        {

        }

        public UserVM(UserDTO row)
        {
            Id = row.Id;
            EmailAdress = row.EmailAdress;
            Password = row.Password;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле почта должно быть заполнено")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Почта")]
        public string EmailAdress { get; set; }

        [Required(ErrorMessage = "Поле пароль должно быть заполнено")]
        [DisplayName("Пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 6 символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
