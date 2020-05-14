using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Models.DTO
{
    public class GroupInRoleDTO : DTOModelBase
    {
        public int GroupId { get; set; }
        public int RoleId { get; set; }
    }
}