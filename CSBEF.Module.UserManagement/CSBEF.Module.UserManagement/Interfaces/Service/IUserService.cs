using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models.HelperModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Interfaces.Service
{
    public interface IUserService : IServiceBase<User, UserDTO>
    {
        Task<IReturnModel<bool>> ChangePassword(ServiceParamsWithIdentifier<ChangePasswordModel> args);
        Task<IReturnModel<string>> ChangePicture(ServiceParamsWithIdentifier<ChangePictureModel> args);
        Task<IReturnModel<string>> ChangeBgPicture(ServiceParamsWithIdentifier<ChangePictureModel> args);
        Task<IReturnModel<IList<UserForCurrentUser>>> UserListForCurrentUser(ServiceParamsWithIdentifier<int> args);
        Task<IReturnModel<bool>> ChangeProfileInformations(ServiceParamsWithIdentifier<ChangeProfileInformationsModel> args);
    }
}