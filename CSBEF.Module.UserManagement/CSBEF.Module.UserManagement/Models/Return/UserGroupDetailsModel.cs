using CSBEF.Module.UserManagement.Models.DTO;
using System.Collections.Generic;

namespace CSBEF.Module.UserManagement.Models.Return
{
    public class UserGroupDetailsModel : GroupDTO
    {
        public List<int> Roles { get; set; } = new List<int>();
        public List<int> Users { get; set; } = new List<int>();
    }
}
