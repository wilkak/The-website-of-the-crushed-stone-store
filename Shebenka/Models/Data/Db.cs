using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Shebenka.Models.Data
{
    public class Db : DbContext
    {
        public DbSet<UserRoleDTO> UserRoles { get; set; }
        public DbSet<UserDTO> Users { get; set; }
        public DbSet<RoleDTO> Roles { get; set; }
        public DbSet<ProductDTO> Products { get; set; }
        public DbSet<ZvonkiDTO> Zvonki { get; set; }

    }
}