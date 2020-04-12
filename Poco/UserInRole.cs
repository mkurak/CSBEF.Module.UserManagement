using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Poco {
    public partial class UserInRole : EntityModelBase {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}