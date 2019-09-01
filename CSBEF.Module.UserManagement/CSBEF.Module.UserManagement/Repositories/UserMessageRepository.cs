using AutoMapper;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement.Repositories
{
    public class UserMessageRepository : RepositoryBase<UserMessage>, IUserMessageRepository
    {
        public UserMessageRepository(ModularDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}