using CSBEF.Core.Interfaces;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement.Interfaces.Repository {
    public interface ITokenRepository : IRepositoryBase<Token> {
        bool KillUserTokens (int userId, int processUserId);
    }
}