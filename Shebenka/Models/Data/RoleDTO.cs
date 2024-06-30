using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shebenka.Models.Data
{
    [Table("TableRoles")]
    public class RoleDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}