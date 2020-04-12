namespace CSBEF.Module.UserManagement.Enums.Errors {
    public enum UserErrorsEnum {
        ChangePasswordModelValidationFail,
        ChangePasswordUserNotFound,
        ChangePasswordWrongCurrentPassword,
        ChangePictureModelValidationFail,
        ChangePictureUserNotFound,
        UserListForCurrentUserUserNotFound,
        ChangeProfileInformationsModelValidationFail,
        ChangeProfileInformationsDataFound,
        SaveModelValidationFail,
        SaveEmailExists,
        SaveUserNameExists,
        SaveUserNotFound,
        SavePasswordRequired,
        SaveCurrentPasswordWrong,
        ChangeStatusModelValidationFail,
        ChangeStatusDataNotFound,
        SaveUserInGroupsModelValidationFail,
        SaveUserInRolesModelValidationFail
    }
}