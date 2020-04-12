using System.ComponentModel.DataAnnotations;

namespace CSBEF.Module.UserManagement.Models.Request {
    public class SaveUserModel {
        [Range (minimum: 0, maximum: int.MaxValue, ErrorMessage = "ModelValidationError_IdWrongValue")]
        public int Id { get; set; }

        [Required (ErrorMessage = "ModelValidationError_NameRequired")]
        [StringLength (maximumLength: 256, ErrorMessage = "ModelValidationError_NameMaxLen")]
        public string Name { get; set; }

        [Required (ErrorMessage = "ModelValidationError_SurnameRequired")]
        [StringLength (maximumLength: 256, ErrorMessage = "ModelValidationError_SurnameMaxLen")]
        public string Surname { get; set; }

        [Required (ErrorMessage = "ModelValidationError_UserNameRequired")]
        [StringLength (maximumLength: 32, ErrorMessage = "ModelValidationError_UserNameMaxLen")]
        public string UserName { get; set; }

        [Required (ErrorMessage = "ModelValidationError_EmailAddressRequired")]
        [EmailAddress (ErrorMessage = "ModelValidationError_EmailAddressWrongFormat")]
        [StringLength (maximumLength: 256, ErrorMessage = "ModelValidationError_EmailAddressMaxLen")]
        public string Email { get; set; }

        [DataType (DataType.Password, ErrorMessage = "ModelValidationError_CurrentPasswordWrongType")]
        public string CurrentPassword { get; set; }

        [DataType (DataType.Password, ErrorMessage = "ModelValidationError_NewPasswordWrongType")]
        public string NewPassword { get; set; }
    }
}