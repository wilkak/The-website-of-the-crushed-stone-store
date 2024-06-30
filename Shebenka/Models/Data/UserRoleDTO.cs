using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shebenka.Models.Data
{
    [Table("TableUserRoles")]
    public class UserRoleDTO
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int RolesId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserDTO User { get; set; }
        [ForeignKey("RolesId")]
        public virtual RoleDTO Role { get; set; }

    }
}