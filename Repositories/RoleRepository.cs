using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement.Repositories {
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository {
        public RoleRepository (ModularDbContext context) : base (context) { }
    }
}