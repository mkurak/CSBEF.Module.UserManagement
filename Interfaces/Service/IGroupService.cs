using System.Collections.Generic;
using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement.Interfaces.Service {
    public interface IGroupService : IServiceBase<Group, GroupDTO> {
        IReturnModel<IList<UserGroupDetailsModel>> ListWithDetails (ServiceParamsWithIdentifier<ActionFilterModel> args);

        IReturnModel<GroupDTO> Save (ServiceParamsWithIdentifier<SaveGroupModel> args);

        IReturnModel<bool> ChangeStatus (ServiceParamsWithIdentifier<ChangeStatusModel> args);

        IReturnModel<bool> SaveGroupInRoles (ServiceParamsWithIdentifier<SaveGroupInRoleModel> args);
    }
}