using CSBEF.Core.Models;
using System;

namespace CSBEF.Module.UserManagement.Poco
{
    public partial class Token : EntityModelBase
    {
        public int UserId { get; set; }
        public string NotificationToken { get; set; }
        public string TokenCode { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string Device { get; set; }
        public string DeviceKey { get; set; }
    }
}