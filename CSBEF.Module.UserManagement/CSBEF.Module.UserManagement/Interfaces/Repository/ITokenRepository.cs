using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Interfaces;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Interfaces.Repository
{
    public interface ITokenRepository : IRepositoryBase<Token>
    {
        Task<bool> KillUserTokens(int userId, int processUserId);
    }
}