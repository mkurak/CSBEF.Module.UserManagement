using System;

namespace CSBEF.Module.UserManagement.Models.Return
{
    public class MessageModelForCurrentUser
    {
        public int Id { get; set; }
        public bool OwnerMe { get; set; }
        public string Message { get; set; }
        public bool ViewStatus { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? ViewDate { get; set; }
    }
}