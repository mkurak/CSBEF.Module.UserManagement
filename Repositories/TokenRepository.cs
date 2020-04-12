using System;
using System.Linq;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement.Repositories {
    public class TokenRepository : RepositoryBase<Token>, ITokenRepository {
        public TokenRepository (ModularDbContext context) : base (context) { }

        public bool KillUserTokens (int userId, int processUserId) {
            var findAllTokens = FindAll (i => i.UserId == userId);
            if (findAllTokens.Any ()) {
                foreach (var token in findAllTokens) {
                    token.Status = false;
                    token.UpdatingDate = DateTime.Now;
                    token.UpdatingUserId = processUserId;
                    Update (token);
                }

                Save ();
            }

            return true;
        }
    }
}