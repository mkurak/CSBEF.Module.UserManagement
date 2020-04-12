using System.ComponentModel.DataAnnotations;

namespace CSBEF.Module.UserManagement.Models.Request {
    public class SaveGroupModel {
        [Range (minimum: 0, maximum: int.MaxValue, ErrorMessage = "ModelValidationError_IdWrongValue")]
        public int Id { get; set; }

        [Required (ErrorMessage = "ModelValidationError_GroupNameRequired")]
        [StringLength (maximumLength: 256, ErrorMessage = "ModelValidationError_GroupNameMaxLen")]
        public string GroupName { get; set; }
    }
}