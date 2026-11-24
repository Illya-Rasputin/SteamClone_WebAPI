using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserRoleEntity: IdentityUserRole<string>
    {
        public UserEntity? User { get; set; }
        public RoleEntity? Role { get; set; }
    }
}
