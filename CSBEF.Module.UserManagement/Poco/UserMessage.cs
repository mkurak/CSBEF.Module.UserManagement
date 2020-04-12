using System;
using CSBEF.Core.Models;

namespace CSBEF.Module.UserManagement.Poco {
    public partial class UserMessage : EntityModelBase {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public bool ViewStatus { get; set; }
        public DateTime? ViewDate { get; set; }
    }
}