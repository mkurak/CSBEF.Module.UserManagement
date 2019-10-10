using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Poco
{
    public partial class GroupInRole : EntityModelBase
    {
        public int GroupId { get; set; }
        public int RoleId { get; set; }
    }
}