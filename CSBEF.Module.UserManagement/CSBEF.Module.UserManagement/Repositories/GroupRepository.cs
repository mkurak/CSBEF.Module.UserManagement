using AutoMapper;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;

namespace CSBEF.Module.UserManagement.Repositories
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(ModularDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}