using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Poco
{
    public class UserInRole : EntityModelBase
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}