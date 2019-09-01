using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Module.UserManagement.Poco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Interfaces.Service
{
    public interface IGroupService : IServiceBase<Group, GroupDTO>
    {
        Task<IReturnModel<IList<UserGroupDetailsModel>>> ListWithDetails(ServiceParamsWithIdentifier<ActionFilterModel> args);

        Task<IReturnModel<GroupDTO>> Save(ServiceParamsWithIdentifier<SaveGroupModel> args);

        Task<IReturnModel<bool>> ChangeStatus(ServiceParamsWithIdentifier<ChangeStatusModel> args);

        Task<IReturnModel<bool>> SaveGroupInRoles(ServiceParamsWithIdentifier<SaveGroupInRoleModel> args);
    }
}