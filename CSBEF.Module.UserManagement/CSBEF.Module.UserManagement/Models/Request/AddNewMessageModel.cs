namespace CSBEF.Module.UserManagement.Models.Request
{
    public class AddNewMessageModel
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
    }
}