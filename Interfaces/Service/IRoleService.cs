using CSBEF.Core.Interfaces;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement.Interfaces.Service {
    public interface IRoleService : IServiceBase<Role, RoleDTO> { }
}