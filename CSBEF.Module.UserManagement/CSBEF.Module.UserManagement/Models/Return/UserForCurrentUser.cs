using CSBEF.Module.UserManagement.Models.DTO;
using System;
using System.Collections.Generic;

namespace CSBEF.Module.UserManagement.Models.Return
{
    public class UserForCurrentUser : UserDTO
    {
        public DateTime LastMessage { get; set; } = DateTime.MinValue;
        public IList<MessageModelForCurrentUser> Messages { get; set; }

        public UserForCurrentUser()
        {
            Messages = new List<MessageModelForCurrentUser>();
        }
    }
}
