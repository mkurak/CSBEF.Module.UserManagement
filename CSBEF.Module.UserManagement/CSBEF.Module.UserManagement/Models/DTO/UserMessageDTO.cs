using CSBEF.Core.Models;
using System;

namespace CSBEF.Module.UserManagement.Models.DTO
{
    public class UserMessageDTO : DTOModelBase
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public bool ViewStatus { get; set; }
        public DateTime? ViewDate { get; set; }
    }
}