using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Poco
{
    public class GroupInRole : EntityModelBase
    {
        public int GroupId { get; set; }
        public int RoleId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Role Role { get; set; }
    }
}