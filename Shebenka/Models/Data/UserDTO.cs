using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Shebenka.Models.Data
{
    [Table("TableUsers")]
    public class UserDTO
    {
        [Key]
        public int Id { get; set; }
        public string EmailAdress { get; set; }
        public string Password { get; set; }
    }
}