using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Models.DTO
{
    public class UserInRoleDTO : DTOModelBase
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}