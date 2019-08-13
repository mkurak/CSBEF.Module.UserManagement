using CSBEF.Core.Models;
using System.Collections.Generic;

namespace CSBEF.Module.UserManagement.Poco
{
    public class Group : EntityModelBase
    {
        public Group()
        {
            GroupInRole = new HashSet<GroupInRole>();
            UserInGroup = new HashSet<UserInGroup>();
        }

        public string GroupName { get; set; }

        public virtual ICollection<GroupInRole> GroupInRole { get; set; }
        public virtual ICollection<UserInGroup> UserInGroup { get; set; }
    }
}