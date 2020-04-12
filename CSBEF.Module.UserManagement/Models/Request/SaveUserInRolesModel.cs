using System.ComponentModel.DataAnnotations;

namespace CSBEF.Module.UserManagement.Models.Request {
    public class SaveUserInRolesModel {
        [Required (ErrorMessage = "ModelValidationError_UserIdRequired")]
        [Range (minimum: 0, maximum: int.MaxValue, ErrorMessage = "ModelValidationError_UserIdWrongValue")]
        public int UserId { get; set; }

        public string Roles { get; set; }
    }
}