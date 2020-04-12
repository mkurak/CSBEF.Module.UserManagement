using System.Collections.Generic;
using CSBEF.Module.UserManagement.Models.DTO;

namespace CSBEF.Module.UserManagement.Models.Return {
    public class UserGroupDetailsModel : GroupDTO {
        public List<int> Roles { get; } = new List<int> ();
        public List<int> Users { get; } = new List<int> ();
    }
}