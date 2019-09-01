using CSBEF.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace CSBEF.Module.UserManagement.Models.Request
{
    public class ChangeProfileInformationsModel : HashControlModel
    {
        [Required(ErrorMessage = "ModelValidationError_IdRequired")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "ModelValidationError_IdIsZero")]
        public int Id { get; set; }

        [Required(ErrorMessage = "ModelValidationError_NameRequired")]
        [StringLength(maximumLength: 256, ErrorMessage = "ModelValidationError_NameMaxLen")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ModelValidationError_SurnameRequired")]
        [StringLength(maximumLength: 256, ErrorMessage = "ModelValidationError_SurnameMaxLen")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "ModelValidationError_ProfileStatusMessageRequired")]
        [StringLength(maximumLength: 512, ErrorMessage = "ModelValidationError_ProfileStatusMessageMaxLen")]
        public string ProfileStatusMessage { get; set; }
    }
}