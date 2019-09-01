using AutoMapper;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Module.UserManagement.Enums.Errors;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Module.UserManagement.Poco;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Services
{
    public class UserMessageService : ServiceBase<UserMessage, UserMessageDTO>, IUserMessageService
    {
        #region Dependencies

        private IHubContext<GlobalHub> _globalHub;
        private IUserRepository _userRepository;

        #endregion Dependencies

        #region ctor

        public UserMessageService(
           IHostingEnvironment hostingEnvironment,
           IConfiguration configuration,
           ILogger<ILog> logger,
           IMapper mapper,
           IUserMessageRepository repository,
           IEventService eventService,

            // Other Repository Dependencies
            IUserRepository userRepository,
            // Other Service Dependencies
            IHubContext<GlobalHub> globalHub
        ) : base(
           hostingEnvironment,
           configuration,
           logger,
           mapper,
           repository,
           eventService,
           "UserManagement",
           "UserMessageService"
        )
        {
            _userRepository = userRepository;
            _globalHub = globalHub;
        }

        #endregion ctor

        #region Actions

        public async Task<IReturnModel<bool>> SaveViewMessage(ServiceParamsWithIdentifier<ViewMessageModel> args)
        {
            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            try
            {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ViewMessageModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                UserMessage getMessage = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.SaveViewMessage.Before").EventHandler<bool, ServiceParamsWithIdentifier<ViewMessageModel>>(args);
                if (beforeEventHandler != null)
                {
                    if (beforeEventHandler.Error.Status)
                    {
                        rtn.Error = beforeEventHandler.Error;
                        cnt = false;
                    }
                }

                #endregion Before Event Handler

                #region Action Body

                if (cnt)
                {
                    getMessage = await Repository.FindAsync(i => i.Id == args.Param.MessageId);
                    if (getMessage == null)
                    {
                        rtn = rtn.SendError(UserMessageErrorsEnum.SaveViewMessage_MessageNotFound);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getMessage.ViewStatus = true;
                    getMessage.ViewDate = DateTime.Now;
                    getMessage.UpdatingUserId = args.UserId;
                    getMessage.UpdatingDate = DateTime.Now;

                    Repository.Update(getMessage);
                    await Repository.SaveAsync();

                    var sendHubMessageModel = new HubViewedMessageModel
                    {
                        FromUserId = getMessage.FromUserId,
                        ToUserId = getMessage.ToUserId,
                        MessageId = getMessage.Id
                    };

                    await _globalHub.Clients.Group("user_" + getMessage.FromUserId).SendAsync($"{ModuleName}.{ServiceName}.ViewedMessage", sendHubMessageModel);
                    await _globalHub.Clients.Group("user_" + getMessage.ToUserId).SendAsync($"{ModuleName}.{ServiceName}.ViewedMessage", sendHubMessageModel);
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ViewMessageModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "SaveViewMessage"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.SaveViewMessage.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ViewMessageModel>>>(afterEventParameterModel);
                    if (afterEventHandler != null)
                    {
                        if (afterEventHandler.Error.Status)
                        {
                            rtn.Error = afterEventHandler.Error;
                            cnt = false;
                        }
                        else
                        {
                            rtn.Result = afterEventHandler.Result;
                        }
                    }
                }

                #endregion After Event Handler

                #region Clear Memory

                args = null;
                beforeEventHandler = null;
                afterEventParameterModel = null;
                afterEventHandler = null;
                getMessage = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<bool>> AddNewMessage(ServiceParamsWithIdentifier<AddNewMessageModel> args)
        {
            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            try
            {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<AddNewMessageModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                User getFromUser = null;
                User getToUser = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.AddNewMessage.Before").EventHandler<bool, ServiceParamsWithIdentifier<AddNewMessageModel>>(args);
                if (beforeEventHandler != null)
                {
                    if (beforeEventHandler.Error.Status)
                    {
                        rtn.Error = beforeEventHandler.Error;
                        cnt = false;
                    }
                }

                #endregion Before Event Handler

                #region Action Body

                if (cnt)
                {
                    if (args.Param.FromUserId != args.UserId)
                    {
                        rtn = rtn.SendError(UserMessageErrorsEnum.AddNewMessage_FromUserWrong);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getFromUser = await _userRepository.FindAsync(i => i.Id == args.Param.FromUserId);

                    if (getFromUser == null)
                    {
                        rtn = rtn.SendError(UserMessageErrorsEnum.AddNewMessage_FromUserNotFound);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getToUser = await _userRepository.FindAsync(i => i.Id == args.Param.ToUserId);

                    if (getToUser == null)
                    {
                        rtn = rtn.SendError(UserMessageErrorsEnum.AddNewMessage_ToUserNotFound);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    var newMessageModel = new UserMessage
                    {
                        FromUserId = getFromUser.Id,
                        ToUserId = getToUser.Id,
                        Message = args.Param.Message,
                        ViewStatus = false,
                        Status = true,
                        ViewDate = null,
                        AddingDate = DateTime.Now,
                        UpdatingDate = DateTime.Now,
                        AddingUserId = args.UserId,
                        UpdatingUserId = args.UserId
                    };

                    newMessageModel = Repository.Add(newMessageModel);
                    await Repository.SaveAsync();

                    var hubSendDataModel = new NewMessageModel
                    {
                        FromUserId = newMessageModel.FromUserId,
                        ToUserId = newMessageModel.ToUserId,
                        MessageId = newMessageModel.Id,
                        Message = newMessageModel.Message,
                        SendDate = newMessageModel.AddingDate
                    };

                    await _globalHub.Clients.Group("user_" + getFromUser.Id).SendAsync($"{ModuleName}.{ServiceName}.NewMessage", hubSendDataModel);
                    await _globalHub.Clients.Group("user_" + getToUser.Id).SendAsync($"{ModuleName}.{ServiceName}.NewMessage", hubSendDataModel);
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<AddNewMessageModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "AddNewMessage"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.AddNewMessage.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<AddNewMessageModel>>>(afterEventParameterModel);
                    if (afterEventHandler != null)
                    {
                        if (afterEventHandler.Error.Status)
                        {
                            rtn.Error = afterEventHandler.Error;
                            cnt = false;
                        }
                        else
                        {
                            rtn.Result = afterEventHandler.Result;
                        }
                    }
                }

                #endregion After Event Handler

                #region Clear Memory

                args = null;
                beforeEventHandler = null;
                afterEventParameterModel = null;
                afterEventHandler = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        #endregion Actions
    }
}