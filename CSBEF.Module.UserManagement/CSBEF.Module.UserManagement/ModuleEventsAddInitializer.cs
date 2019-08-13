using CSBEF.Core.Enums;
using CSBEF.Core.Interfaces;

namespace CSBEF.Module.UserManagement
{
    public class ModuleEventsAddInitializer : IModuleEventsAddInitializer
    {
        public ModuleEventsAddInitializer()
        {
        }

        public void Start(IEventService eventService)
        {
            #region TokenService

            #region Base Actions

            eventService.AddEvent("TokenService.FirstAsync.Before", "UserManagement", "TokenService", "FirstAsync", EventTypeEnum.before);
            eventService.AddEvent("TokenService.FirstOrDefaultAsync.Before", "UserManagement", "TokenService", "FirstOrDefaultAsync", EventTypeEnum.before);
            eventService.AddEvent("TokenService.AnyAsync.Before", "UserManagement", "TokenService", "AnyAsync", EventTypeEnum.before);
            eventService.AddEvent("TokenService.ListAsync.Before", "UserManagement", "TokenService", "ListAsync", EventTypeEnum.before);

            eventService.AddEvent("TokenService.FirstAsync.After", "UserManagement", "TokenService", "FirstAsync", EventTypeEnum.after);
            eventService.AddEvent("TokenService.FirstOrDefaultAsync.After", "UserManagement", "TokenService", "FirstOrDefaultAsync", EventTypeEnum.after);
            eventService.AddEvent("TokenService.AnyAsync.After", "UserManagement", "TokenService", "AnyAsync", EventTypeEnum.after);
            eventService.AddEvent("TokenService.ListAsync.After", "UserManagement", "TokenService", "ListAsync", EventTypeEnum.after);

            #endregion Base Actions

            #region Service Actions

            eventService.AddEvent("TokenService.CreateToken.Before", "UserManagement", "TokenService", "CreateToken", EventTypeEnum.before);
            eventService.AddEvent("TokenService.CheckToken.Before", "UserManagement", "TokenService", "CheckToken", EventTypeEnum.before);

            eventService.AddEvent("TokenService.CreateToken.After", "UserManagement", "TokenService", "CreateToken", EventTypeEnum.after);
            eventService.AddEvent("TokenService.CheckToken.After", "UserManagement", "TokenService", "CheckToken", EventTypeEnum.after);

            #endregion Service Actions

            #endregion TokenService

            #region UserService

            #region Base Actions

            eventService.AddEvent("UserService.FirstAsync.Before", "UserManagement", "UserService", "FirstAsync", EventTypeEnum.before);
            eventService.AddEvent("UserService.FirstOrDefaultAsync.Before", "UserManagement", "UserService", "FirstOrDefaultAsync", EventTypeEnum.before);
            eventService.AddEvent("UserService.AnyAsync.Before", "UserManagement", "UserService", "AnyAsync", EventTypeEnum.before);
            eventService.AddEvent("UserService.ListAsync.Before", "UserManagement", "UserService", "ListAsync", EventTypeEnum.before);

            eventService.AddEvent("UserService.FirstAsync.After", "UserManagement", "UserService", "FirstAsync", EventTypeEnum.after);
            eventService.AddEvent("UserService.FirstOrDefaultAsync.After", "UserManagement", "UserService", "FirstOrDefaultAsync", EventTypeEnum.after);
            eventService.AddEvent("UserService.AnyAsync.After", "UserManagement", "UserService", "AnyAsync", EventTypeEnum.after);
            eventService.AddEvent("UserService.ListAsync.After", "UserManagement", "UserService", "ListAsync", EventTypeEnum.after);

            #endregion Base Actions

            #region Service Actions

            eventService.AddEvent("UserService.UserListForCurrentUser.Before", "UserManagement", "UserService", "UserListForCurrentUser", EventTypeEnum.before);

            eventService.AddEvent("UserService.UserListForCurrentUser.After", "UserManagement", "UserService", "UserListForCurrentUser", EventTypeEnum.after);

            #endregion

            #region Service Actions

            eventService.AddEvent("UserService.ChangePassword.Before", "UserManagement", "UserService", "ChangePassword", EventTypeEnum.before);
            eventService.AddEvent("UserService.ChangePicture.Before", "UserManagement", "UserService", "ChangePicture", EventTypeEnum.before);
            eventService.AddEvent("UserService.ChangeBgPicture.Before", "UserManagement", "UserService", "ChangeBgPicture", EventTypeEnum.before);

            eventService.AddEvent("UserService.ChangePassword.After", "UserManagement", "UserService", "ChangePassword", EventTypeEnum.after);
            eventService.AddEvent("UserService.ChangePicture.After", "UserManagement", "UserService", "ChangePicture", EventTypeEnum.after);
            eventService.AddEvent("UserService.ChangeBgPicture.After", "UserManagement", "UserService", "ChangeBgPicture", EventTypeEnum.after);

            #endregion Service Actions

            #endregion UserService

            #region UserMessageService

            #region Base Actions

            eventService.AddEvent("UserMessageService.FirstAsync.Before", "UserManagement", "UserMessageService", "FirstAsync", EventTypeEnum.before);
            eventService.AddEvent("UserMessageService.FirstOrDefaultAsync.Before", "UserManagement", "UserMessageService", "FirstOrDefaultAsync", EventTypeEnum.before);
            eventService.AddEvent("UserMessageService.AnyAsync.Before", "UserManagement", "UserMessageService", "AnyAsync", EventTypeEnum.before);
            eventService.AddEvent("UserMessageService.ListAsync.Before", "UserManagement", "UserMessageService", "ListAsync", EventTypeEnum.before);

            eventService.AddEvent("UserMessageService.FirstAsync.After", "UserManagement", "UserMessageService", "FirstAsync", EventTypeEnum.after);
            eventService.AddEvent("UserMessageService.FirstOrDefaultAsync.After", "UserManagement", "UserMessageService", "FirstOrDefaultAsync", EventTypeEnum.after);
            eventService.AddEvent("UserMessageService.AnyAsync.After", "UserManagement", "UserMessageService", "AnyAsync", EventTypeEnum.after);
            eventService.AddEvent("UserMessageService.ListAsync.After", "UserManagement", "UserMessageService", "ListAsync", EventTypeEnum.after);

            #endregion Base Actions

            #region Service Actions

            eventService.AddEvent("UserMessageService.SaveViewMessage.Before", "UserManagement", "UserMessageService", "SaveViewMessage", EventTypeEnum.before);

            eventService.AddEvent("UserMessageService.SaveViewMessage.After", "UserManagement", "UserMessageService", "SaveViewMessage", EventTypeEnum.after);

            #endregion Service Actions

            #endregion UserMessageService
        }
    }
}