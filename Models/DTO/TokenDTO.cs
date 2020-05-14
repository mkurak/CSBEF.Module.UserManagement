using CSBEF.Core.Models;
using System;

namespace CSBEF.Module.UserManagement.Models.DTO
{
    public class TokenDTO : DTOModelBase
    {
        public int UserId { get; set; }
        public string NotificationToken { get; set; }
        public string TokenCode { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string Device { get; set; }
        public string DeviceKey { get; set; }
    }
}