using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Models.DTO
{
    public class UserDTO : DTOModelBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ProfilePic { get; set; }
        public string ProfileBgPic { get; set; }
        public string ProfileStatusMessage { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}