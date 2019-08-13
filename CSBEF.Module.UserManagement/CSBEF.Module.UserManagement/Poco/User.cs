using CSBEF.Core.Models;
using System.Collections.Generic;

namespace CSBEF.Module.UserManagement.Poco
{
    public class User : EntityModelBase
    {
        public User()
        {
            Token = new HashSet<Token>();
            UserInGroup = new HashSet<UserInGroup>();
            UserInRole = new HashSet<UserInRole>();
            UserMessageFromUser = new HashSet<UserMessage>();
            UserMessageToUser = new HashSet<UserMessage>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string ProfilePic { get; set; }
        public string ProfileBgPic { get; set; }
        public string ProfileStatusMessage { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Token> Token { get; set; }
        public virtual ICollection<UserInGroup> UserInGroup { get; set; }
        public virtual ICollection<UserInRole> UserInRole { get; set; }
        public virtual ICollection<UserMessage> UserMessageFromUser { get; set; }
        public virtual ICollection<UserMessage> UserMessageToUser { get; set; }
    }
}