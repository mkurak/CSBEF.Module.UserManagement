using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Core.Models.HelperModels;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Module.UserManagement.Poco;
using System.Collections.Generic;

namespace CSBEF.Module.UserManagement.Interfaces.Service
{
    public interface IUserService : IServiceBase<User, UserDTO>
    {
        IReturnModel<bool> ChangePassword(ServiceParamsWithIdentifier<ChangePasswordModel> args);

        IReturnModel<string> ChangePicture(ServiceParamsWithIdentifier<ChangePictureModel> args);

        IReturnModel<string> ChangeBgPicture(ServiceParamsWithIdentifier<ChangePictureModel> args);

        IReturnModel<IList<UserForCurrentUser>> UserListForCurrentUser(ServiceParamsWithIdentifier<int> args);

        IReturnModel<bool> ChangeProfileInformations(ServiceParamsWithIdentifier<ChangeProfileInformationsModel> args);

        IReturnModel<IList<UserDetailsModel>> ListWithDetails(ServiceParamsWithIdentifier<ActionFilterModel> args);

        IReturnModel<UserDTO> Save(ServiceParamsWithIdentifier<SaveUserModel> args);

        IReturnModel<bool> ChangeStatus(ServiceParamsWithIdentifier<ChangeStatusModel> args);

        IReturnModel<bool> SaveUserInGroups(ServiceParamsWithIdentifier<SaveUserInGroupsModel> args);

        IReturnModel<bool> SaveUserInRoles(ServiceParamsWithIdentifier<SaveUserInRolesModel> args);
    }
}