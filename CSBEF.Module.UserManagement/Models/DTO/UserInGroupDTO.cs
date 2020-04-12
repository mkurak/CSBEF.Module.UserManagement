using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Models.DTO {
    public class UserInGroupDTO : DTOModelBase {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}