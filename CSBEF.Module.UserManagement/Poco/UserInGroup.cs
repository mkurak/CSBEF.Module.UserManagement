using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Poco {
    public partial class UserInGroup : EntityModelBase {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}