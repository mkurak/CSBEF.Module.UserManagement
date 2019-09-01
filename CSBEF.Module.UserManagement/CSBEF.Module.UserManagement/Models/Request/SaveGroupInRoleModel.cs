using CSBEF.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace CSBEF.Module.UserManagement.Models.Request
{
    public class SaveGroupInRoleModel : HashControlModel
    {
        [Required(ErrorMessage = "ModelValidationError_GroupIdRequired")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "ModelValidationError_GroupIdWrongValue")]
        public int GroupId { get; set; }

        public string Roles { get; set; }
    }
}