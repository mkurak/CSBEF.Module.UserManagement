using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement.Repositories {
    public class UserRepository : RepositoryBase<User>, IUserRepository {
        public UserRepository (ModularDbContext context) : base (context) { }
    }
}