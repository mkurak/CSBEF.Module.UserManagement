namespace CSBEF.Module.UserManagement.Enums.Errors
{
    public enum UserErrorsEnum
    {
        ChangePassword_ModelValidationFail,
        ChangePassword_UserNotFound,
        ChangePassword_WrongCurrentPassword,
        ChangePicture_ModelValidationFail,
        ChangePicture_UserNotFound,
        UserListForCurrentUser_UserNotFound,
        ChangeProfileInformations_ModelValidationFail,
        ChangeProfileInformations_DataFound,
        Save_ModelValidationFail,
        Save_EmailExists,
        Save_UserNameExists,
        Save_UserNotFound,
        Save_PasswordRequired,
        Save_CurrentPasswordWrong,
        ChangeStatus_ModelValidationFail,
        ChangeStatus_DataNotFound,
        SaveUserInGroups_ModelValidationFail,
        SaveUserInRoles_ModelValidationFail
    }
}