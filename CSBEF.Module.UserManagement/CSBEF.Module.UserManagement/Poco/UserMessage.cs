using CSBEF.Core.Models;
using System;

namespace CSBEF.Module.UserManagement.Poco
{
    public class UserMessage : EntityModelBase
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public bool ViewStatus { get; set; }
        public DateTime? ViewDate { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}