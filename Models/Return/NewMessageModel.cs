using System;

namespace CSBEF.Module.UserManagement.Models.Return
{
    public class NewMessageModel
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int MessageId { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
    }
}