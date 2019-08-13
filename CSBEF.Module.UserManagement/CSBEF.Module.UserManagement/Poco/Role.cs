using CSBEF.Core.Models;
using System.Collections.Generic;

namespace CSBEF.Module.UserManagement.Poco
{
    public class Role : EntityModelBase
    {
        public Role()
        {
            GroupInRole = new HashSet<GroupInRole>();
            UserInRole = new HashSet<UserInRole>();
        }

        public string RoleName { get; set; }
        public string RoleTitle { get; set; }
        public string RoleDescription { get; set; }

        public virtual ICollection<GroupInRole> GroupInRole { get; set; }
        public virtual ICollection<UserInRole> UserInRole { get; set; }
    }
}