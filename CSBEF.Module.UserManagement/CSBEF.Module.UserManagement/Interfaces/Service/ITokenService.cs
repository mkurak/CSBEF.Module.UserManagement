using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Poco;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Interfaces.Service
{
    public interface ITokenService : IServiceBase<Token, TokenDTO>
    {
        Task<IReturnModel<string>> CreateToken(ServiceParamsWithIdentifier<CreateTokenModel> args);

        Task<IReturnModel<bool>> CheckToken(ServiceParamsWithIdentifier<string> args);
    }
}