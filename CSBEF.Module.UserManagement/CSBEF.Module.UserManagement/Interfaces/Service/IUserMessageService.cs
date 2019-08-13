using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Interfaces.Service
{
    public interface IUserMessageService : IServiceBase<UserMessage, UserMessageDTO>
    {
        Task<IReturnModel<bool>> SaveViewMessage(ServiceParamsWithIdentifier<ViewMessageModel> args);

        Task<IReturnModel<bool>> AddNewMessage(ServiceParamsWithIdentifier<AddNewMessageModel> args);
    }
}