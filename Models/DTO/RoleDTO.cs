using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Models.DTO {
    public class RoleDTO : DTOModelBase {
        public string RoleName { get; set; }
        public string RoleTitle { get; set; }
        public string RoleDescription { get; set; }
    }
}