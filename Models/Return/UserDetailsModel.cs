﻿using CSBEF.Module.UserManagement.Models.DTO;
using System.Collections.Generic;

namespace CSBEF.Module.UserManagement.Models.Return
{
    public class UserDetailsModel
    {
        public UserDTO User { get; set; }
        public List<UserInGroupDTO> InGroups { get; } = new List<UserInGroupDTO>();
        public List<UserInRoleModel> InRoles { get; } = new List<UserInRoleModel>();
    }
}