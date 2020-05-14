namespace CSBEF.Module.UserManagement.Models.Request
{
    public class CreateTokenModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Device { get; set; }
        public string DeviceKey { get; set; }
        public string NotificationToken { get; set; }
        public string Hash { get; set; }
    }
}