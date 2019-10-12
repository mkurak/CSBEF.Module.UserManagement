using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Poco;

namespace CSBEF.Module.UserManagement.Interfaces.Service
{
    public interface IUserMessageService : IServiceBase<UserMessage, UserMessageDTO>
    {
        IReturnModel<bool> SaveViewMessage(ServiceParamsWithIdentifier<ViewMessageModel> args);

        IReturnModel<bool> AddNewMessage(ServiceParamsWithIdentifier<AddNewMessageModel> args);
    }
}