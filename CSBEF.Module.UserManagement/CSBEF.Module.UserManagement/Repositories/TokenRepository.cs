using AutoMapper;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Repositories
{
    public class TokenRepository : RepositoryBase<Token>, ITokenRepository
    {
        public TokenRepository(ModularDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> KillUserTokens(int userId, int processUserId)
        {
            var findAllTokens = await FindAllAsync(i => i.UserId == userId);
            if (findAllTokens.Any())
            {
                foreach(var token in findAllTokens)
                {
                    token.Status = false;
                    token.UpdatingDate = DateTime.Now;
                    token.UpdatingUserId = processUserId;
                    Update(token);
                }

                await SaveAsync();
            }

            return true;
        }
    }
}