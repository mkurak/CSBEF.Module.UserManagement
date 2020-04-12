using System.ComponentModel.DataAnnotations;

namespace CSBEF.Module.UserManagement.Models.Request
{
    public class ChangePasswordModel {
        [Required (ErrorMessage = "ModelValidationError_UserIdRequired")]
        [Range (minimum: 1, maximum: int.MaxValue, ErrorMessage = "ModelValidationError_UserIdIsZero")]
        public int UserId { get; set; }

        [Required (ErrorMessage = "ModelValidationError_CurrentPassRequired")]
        public string CurrentPass { get; set; }

        [Required (ErrorMessage = "ModelValidationError_NewPassRequired")]
        public string NewPass { get; set; }
    }
}