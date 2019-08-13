using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Poco
{
    public class UserInGroup : EntityModelBase
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}