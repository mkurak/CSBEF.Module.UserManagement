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
            eventService.AddEvent("UserService.ChangePassword.Before", "UserManagement", "UserService", "ChangePassword", EventTypeEnum.before);
            eventService.AddEvent("UserService.ChangePicture.Before", "UserManagement", "UserService", "ChangePicture", EventTypeEnum.before);
            eventService.AddEvent("UserService.ChangeBgPicture.Before", "UserManagement", "UserService", "ChangeBgPicture", EventTypeEnum.before);
            eventService.AddEvent("UserService.ListWithDetails.Before", "UserManagement", "UserService", "ListWithDetails", EventTypeEnum.before);
            eventService.AddEvent("UserService.Save.Before", "UserManagement", "UserService", "Save", EventTypeEnum.before);
            eventService.AddEvent("UserService.ChangeStatus.Before", "UserManagement", "UserService", "ChangeStatus", EventTypeEnum.before);
            eventService.AddEvent("UserService.SaveUserInGroups.Before", "UserManagement", "UserService", "SaveUserInGroups", EventTypeEnum.before);
            eventService.AddEvent("UserService.SaveUserInRoles.Before", "UserManagement", "UserService", "SaveUserInRoles", EventTypeEnum.before);

            eventService.AddEvent("UserService.ChangePassword.After", "UserManagement", "UserService", "ChangePassword", EventTypeEnum.after);
            eventService.AddEvent("UserService.ChangePicture.After", "UserManagement", "UserService", "ChangePicture", EventTypeEnum.after);
            eventService.AddEvent("UserService.ChangeBgPicture.After", "UserManagement", "UserService", "ChangeBgPicture", EventTypeEnum.after);
            eventService.AddEvent("UserService.UserListForCurrentUser.After", "UserManagement", "UserService", "UserListForCurrentUser", EventTypeEnum.after);
            eventService.AddEvent("UserService.ListWithDetails.After", "UserManagement", "UserService", "ListWithDetails", EventTypeEnum.after);
            eventService.AddEvent("UserService.Save.After", "UserManagement", "UserService", "Save", EventTypeEnum.after);
            eventService.AddEvent("UserService.ChangeStatus.After", "UserManagement", "UserService", "ChangeStatus", EventTypeEnum.after);
            eventService.AddEvent("UserService.SaveUserInGroups.After", "UserManagement", "UserService", "SaveUserInGroups", EventTypeEnum.after);
            eventService.AddEvent("UserService.SaveUserInRoles.After", "UserManagement", "UserService", "SaveUserInRoles", EventTypeEnum.after);

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

            #region TokenService

            #region Base Actions

            eventService.AddEvent("RoleService.FirstAsync.Before", "UserManagement", "RoleService", "FirstAsync", EventTypeEnum.before);
            eventService.AddEvent("RoleService.FirstOrDefaultAsync.Before", "UserManagement", "RoleService", "FirstOrDefaultAsync", EventTypeEnum.before);
            eventService.AddEvent("RoleService.AnyAsync.Before", "UserManagement", "RoleService", "AnyAsync", EventTypeEnum.before);
            eventService.AddEvent("RoleService.ListAsync.Before", "UserManagement", "RoleService", "ListAsync", EventTypeEnum.before);

            eventService.AddEvent("RoleService.FirstAsync.After", "UserManagement", "RoleService", "FirstAsync", EventTypeEnum.after);
            eventService.AddEvent("RoleService.FirstOrDefaultAsync.After", "UserManagement", "RoleService", "FirstOrDefaultAsync", EventTypeEnum.after);
            eventService.AddEvent("RoleService.AnyAsync.After", "UserManagement", "RoleService", "AnyAsync", EventTypeEnum.after);
            eventService.AddEvent("RoleService.ListAsync.After", "UserManagement", "RoleService", "ListAsync", EventTypeEnum.after);

            #endregion Base Actions

            #endregion TokenService

            #region RoleService

            #region Base Actions

            eventService.AddEvent("RoleService.FirstAsync.Before", "UserManagement", "RoleService", "FirstAsync", EventTypeEnum.before);
            eventService.AddEvent("RoleService.FirstOrDefaultAsync.Before", "UserManagement", "RoleService", "FirstOrDefaultAsync", EventTypeEnum.before);
            eventService.AddEvent("RoleService.AnyAsync.Before", "UserManagement", "RoleService", "AnyAsync", EventTypeEnum.before);
            eventService.AddEvent("RoleService.ListAsync.Before", "UserManagement", "RoleService", "ListAsync", EventTypeEnum.before);

            eventService.AddEvent("RoleService.FirstAsync.After", "UserManagement", "RoleService", "FirstAsync", EventTypeEnum.after);
            eventService.AddEvent("RoleService.FirstOrDefaultAsync.After", "UserManagement", "RoleService", "FirstOrDefaultAsync", EventTypeEnum.after);
            eventService.AddEvent("RoleService.AnyAsync.After", "UserManagement", "RoleService", "AnyAsync", EventTypeEnum.after);
            eventService.AddEvent("RoleService.ListAsync.After", "UserManagement", "RoleService", "ListAsync", EventTypeEnum.after);

            #endregion Base Actions

            #endregion RoleService

            #region GroupService

            #region Base Actions

            eventService.AddEvent("GroupService.FirstAsync.Before", "UserManagement", "GroupService", "FirstAsync", EventTypeEnum.before);
            eventService.AddEvent("GroupService.FirstOrDefaultAsync.Before", "UserManagement", "GroupService", "FirstOrDefaultAsync", EventTypeEnum.before);
            eventService.AddEvent("GroupService.AnyAsync.Before", "UserManagement", "GroupService", "AnyAsync", EventTypeEnum.before);
            eventService.AddEvent("GroupService.ListAsync.Before", "UserManagement", "GroupService", "ListAsync", EventTypeEnum.before);

            eventService.AddEvent("GroupService.FirstAsync.After", "UserManagement", "GroupService", "FirstAsync", EventTypeEnum.after);
            eventService.AddEvent("GroupService.FirstOrDefaultAsync.After", "UserManagement", "GroupService", "FirstOrDefaultAsync", EventTypeEnum.after);
            eventService.AddEvent("GroupService.AnyAsync.After", "UserManagement", "GroupService", "AnyAsync", EventTypeEnum.after);
            eventService.AddEvent("GroupService.ListAsync.After", "UserManagement", "GroupService", "ListAsync", EventTypeEnum.after);

            #endregion Base Actions

            #region Service Actions

            eventService.AddEvent("GroupService.ListWithDetails.Before", "UserManagement", "GroupService", "ListWithDetails", EventTypeEnum.before);
            eventService.AddEvent("GroupService.Save.Before", "UserManagement", "GroupService", "Save", EventTypeEnum.before);
            eventService.AddEvent("GroupService.ChangeStatus.Before", "UserManagement", "GroupService", "ChangeStatus", EventTypeEnum.before);
            eventService.AddEvent("GroupService.SaveGroupInRoles.Before", "UserManagement", "GroupService", "SaveGroupInRoles", EventTypeEnum.before);

            eventService.AddEvent("GroupService.ListWithDetails.After", "UserManagement", "GroupService", "ListWithDetails", EventTypeEnum.after);
            eventService.AddEvent("GroupService.Save.After", "UserManagement", "GroupService", "Save", EventTypeEnum.after);
            eventService.AddEvent("GroupService.ChangeStatus.After", "UserManagement", "GroupService", "ChangeStatus", EventTypeEnum.after);
            eventService.AddEvent("GroupService.SaveGroupInRoles.After", "UserManagement", "GroupService", "SaveGroupInRoles", EventTypeEnum.after);

            #endregion Service Actions

            #endregion GroupService
        }
    }
}