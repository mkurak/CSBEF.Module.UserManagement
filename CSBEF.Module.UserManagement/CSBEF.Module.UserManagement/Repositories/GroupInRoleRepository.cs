using AutoMapper;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;

namespace CSBEF.Module.UserManagement.Repositories
{
    public class GroupInRoleRepository : RepositoryBase<GroupInRole>, IGroupInRoleRepository
    {
        public GroupInRoleRepository(ModularDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}