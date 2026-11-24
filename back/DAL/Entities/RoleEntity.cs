using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class RoleEntity : IdentityRole
    {
        public List<UserRoleEntity> UserRoles { get; set; } = [];
    }
}
