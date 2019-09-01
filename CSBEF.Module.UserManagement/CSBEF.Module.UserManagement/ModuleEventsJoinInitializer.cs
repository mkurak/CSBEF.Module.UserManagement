using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Core.Models.HubModels;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.Request;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement
{
    public class ModuleEventsJoinInitializer : IModuleEventsJoinInitializer
    {
        private IServiceProvider _serviceProvicer;

        public ModuleEventsJoinInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvicer = serviceProvider;
        }

        public void Start(IEventService eventService)
        {
            eventService.GetEvent("Main", "InComingToken").Event += MainInComingTokenHandler;
            eventService.GetEvent("Main", "InComingHubClientData").Event += MainInComingHubClientDataHandler;
        }

        private async Task<dynamic> MainInComingTokenHandler(dynamic token, IEventInfo eventInfo)
        {
            var tokenService = _serviceProvicer.GetService<ITokenService>();

            var run = await tokenService.CheckToken(new ServiceParamsWithIdentifier<string>(token as string, 0, 0));
            return run;
        }

        private async Task<dynamic> MainInComingHubClientDataHandler(dynamic data, IEventInfo eventInfo)
        {
            var logger = _serviceProvicer.GetService<ILogger<ILog>>();

            var reData = (ServiceParamsWithIdentifier<InComingClientDataModel>)data;
            var rtn = new ReturnModel<SendModuleDataModel>(logger);
            var userMessageService = _serviceProvicer.GetService<IUserMessageService>();
            if (reData.Param.ModuleName == "UserManagement")
            {
                switch (reData.Param.DataKey)
                {
                    case "ChatViewMessage":
                        var execSaveViewMessageConvertData = JsonConvert.DeserializeObject<ViewMessageModel>(reData.Param.DataJsonString);
                        var execSaveViewMessageParamsData = new ServiceParamsWithIdentifier<ViewMessageModel>(execSaveViewMessageConvertData, reData.UserId, reData.TokenId);
                        var execSaveViewMessage = await userMessageService.SaveViewMessage(execSaveViewMessageParamsData);
                        if (execSaveViewMessage.Error.Status)
                        {
                            rtn.Error = execSaveViewMessage.Error;
                        }
                        else
                        {
                            rtn.Result = new SendModuleDataModel
                            {
                                DataJsonString = JsonConvert.SerializeObject(execSaveViewMessage.Result, Formatting.Indented)
                            };
                        }
                        break;

                    case "AddNewMessage":
                        var execAddNewMessageConvertData = JsonConvert.DeserializeObject<AddNewMessageModel>(reData.Param.DataJsonString);
                        var execAddNewMessageParamsData = new ServiceParamsWithIdentifier<AddNewMessageModel>(execAddNewMessageConvertData, reData.UserId, reData.TokenId);
                        var execAddNewMessage = await userMessageService.AddNewMessage(execAddNewMessageParamsData);
                        if (execAddNewMessage.Error.Status)
                        {
                            rtn.Error = execAddNewMessage.Error;
                        }
                        else
                        {
                            rtn.Result = new SendModuleDataModel
                            {
                                DataJsonString = JsonConvert.SerializeObject(execAddNewMessage.Result, Formatting.Indented)
                            };
                        }
                        break;

                    case "NotifyTest":
                        var hubNotificationService = _serviceProvicer.GetService<IHubNotificationService>();
                        await hubNotificationService.OnNotify(new NotificationModel
                        {
                            Title = "Test Bildirim",
                            Content = "Bu bir test bildirimidir.",
                            AddDate = DateTime.Now,
                            Icon = "notifications",
                            Url = "/messages"
                        });
                        break;
                }
            }

            return rtn;
        }
    }
}