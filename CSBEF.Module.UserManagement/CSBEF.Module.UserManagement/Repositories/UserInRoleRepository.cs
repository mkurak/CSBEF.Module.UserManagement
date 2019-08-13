using AutoMapper;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;

namespace CSBEF.Module.UserManagement.Repositories
{
    public class UserInRoleRepository : RepositoryBase<UserInRole>, IUserInRoleRepository
    {
        public UserInRoleRepository(ModularDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}