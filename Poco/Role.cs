using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Poco {
    public partial class Role : EntityModelBase {
        public string RoleName { get; set; }
        public string RoleTitle { get; set; }
        public string RoleDescription { get; set; }
    }
}