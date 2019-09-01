using CSBEF.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace CSBEF.Module.UserManagement.Models.Request
{
    public class ChangeStatusModel : HashControlModel
    {
        [Required(ErrorMessage = "ModelValidationError_IdRequired")]
        [Range(minimum: 1, maximum: 2147483647, ErrorMessage = "ModelValidationError_IdIsZero")]
        public int Id { get; set; }

        public bool Status { get; set; }
    }
}