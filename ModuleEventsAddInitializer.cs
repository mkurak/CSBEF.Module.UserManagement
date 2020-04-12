using System;
using CSBEF.Core.Enums;
using CSBEF.Core.Interfaces;

namespace CSBEF.Module.UserManagement {
    public class ModuleEventsAddInitializer : IModuleEventsAddInitializer {
        public ModuleEventsAddInitializer () { }

        public void Start (IEventService eventService) {
            if (eventService == null)
                throw new ArgumentNullException (nameof (eventService));

            #region TokenService

            #region Base Actions

            eventService.AddEvent ("TokenService.First.Before", "UserManagement", "TokenService", "First", EventTypeEnum.before);
            eventService.AddEvent ("TokenService.FirstOrDefault.Before", "UserManagement", "TokenService", "FirstOrDefault", EventTypeEnum.before);
            eventService.AddEvent ("TokenService.Any.Before", "UserManagement", "TokenService", "Any", EventTypeEnum.before);
            eventService.AddEvent ("TokenService.List.Before", "UserManagement", "TokenService", "List", EventTypeEnum.before);
            eventService.AddEvent ("TokenService.Count.Before", "UserManagement", "TokenService", "Count", EventTypeEnum.before);

            eventService.AddEvent ("TokenService.First.After", "UserManagement", "TokenService", "First", EventTypeEnum.after);
            eventService.AddEvent ("TokenService.FirstOrDefault.After", "UserManagement", "TokenService", "FirstOrDefault", EventTypeEnum.after);
            eventService.AddEvent ("TokenService.Any.After", "UserManagement", "TokenService", "Any", EventTypeEnum.after);
            eventService.AddEvent ("TokenService.List.After", "UserManagement", "TokenService", "List", EventTypeEnum.after);
            eventService.AddEvent ("TokenService.Count.After", "UserManagement", "TokenService", "Count", EventTypeEnum.after);

            #endregion Base Actions

            #region Service Actions

            eventService.AddEvent ("TokenService.CreateToken.Before", "UserManagement", "TokenService", "CreateToken", EventTypeEnum.before);
            eventService.AddEvent ("TokenService.CheckToken.Before", "UserManagement", "TokenService", "CheckToken", EventTypeEnum.before);

            eventService.AddEvent ("TokenService.CreateToken.After", "UserManagement", "TokenService", "CreateToken", EventTypeEnum.after);
            eventService.AddEvent ("TokenService.CheckToken.After", "UserManagement", "TokenService", "CheckToken", EventTypeEnum.after);

            #endregion Service Actions

            #endregion TokenService

            #region UserService

            #region Base Actions

            eventService.AddEvent ("UserService.First.Before", "UserManagement", "UserService", "First", EventTypeEnum.before);
            eventService.AddEvent ("UserService.FirstOrDefault.Before", "UserManagement", "UserService", "FirstOrDefault", EventTypeEnum.before);
            eventService.AddEvent ("UserService.Any.Before", "UserManagement", "UserService", "Any", EventTypeEnum.before);
            eventService.AddEvent ("UserService.List.Before", "UserManagement", "UserService", "List", EventTypeEnum.before);
            eventService.AddEvent ("UserService.Count.Before", "UserManagement", "UserService", "Count", EventTypeEnum.before);

            eventService.AddEvent ("UserService.First.After", "UserManagement", "UserService", "First", EventTypeEnum.after);
            eventService.AddEvent ("UserService.FirstOrDefault.After", "UserManagement", "UserService", "FirstOrDefault", EventTypeEnum.after);
            eventService.AddEvent ("UserService.Any.After", "UserManagement", "UserService", "Any", EventTypeEnum.after);
            eventService.AddEvent ("UserService.List.After", "UserManagement", "UserService", "List", EventTypeEnum.after);
            eventService.AddEvent ("UserService.Count.After", "UserManagement", "UserService", "Count", EventTypeEnum.after);

            #endregion Base Actions

            #region Service Actions

            eventService.AddEvent ("UserService.UserListForCurrentUser.Before", "UserManagement", "UserService", "UserListForCurrentUser", EventTypeEnum.before);
            eventService.AddEvent ("UserService.ChangePassword.Before", "UserManagement", "UserService", "ChangePassword", EventTypeEnum.before);
            eventService.AddEvent ("UserService.ChangePicture.Before", "UserManagement", "UserService", "ChangePicture", EventTypeEnum.before);
            eventService.AddEvent ("UserService.ChangeBgPicture.Before", "UserManagement", "UserService", "ChangeBgPicture", EventTypeEnum.before);
            eventService.AddEvent ("UserService.ListWithDetails.Before", "UserManagement", "UserService", "ListWithDetails", EventTypeEnum.before);
            eventService.AddEvent ("UserService.Save.Before", "UserManagement", "UserService", "Save", EventTypeEnum.before);
            eventService.AddEvent ("UserService.ChangeStatus.Before", "UserManagement", "UserService", "ChangeStatus", EventTypeEnum.before);
            eventService.AddEvent ("UserService.SaveUserInGroups.Before", "UserManagement", "UserService", "SaveUserInGroups", EventTypeEnum.before);
            eventService.AddEvent ("UserService.SaveUserInRoles.Before", "UserManagement", "UserService", "SaveUserInRoles", EventTypeEnum.before);

            eventService.AddEvent ("UserService.ChangePassword.After", "UserManagement", "UserService", "ChangePassword", EventTypeEnum.after);
            eventService.AddEvent ("UserService.ChangePicture.After", "UserManagement", "UserService", "ChangePicture", EventTypeEnum.after);
            eventService.AddEvent ("UserService.ChangeBgPicture.After", "UserManagement", "UserService", "ChangeBgPicture", EventTypeEnum.after);
            eventService.AddEvent ("UserService.UserListForCurrentUser.After", "UserManagement", "UserService", "UserListForCurrentUser", EventTypeEnum.after);
            eventService.AddEvent ("UserService.ListWithDetails.After", "UserManagement", "UserService", "ListWithDetails", EventTypeEnum.after);
            eventService.AddEvent ("UserService.Save.After", "UserManagement", "UserService", "Save", EventTypeEnum.after);
            eventService.AddEvent ("UserService.ChangeStatus.After", "UserManagement", "UserService", "ChangeStatus", EventTypeEnum.after);
            eventService.AddEvent ("UserService.SaveUserInGroups.After", "UserManagement", "UserService", "SaveUserInGroups", EventTypeEnum.after);
            eventService.AddEvent ("UserService.SaveUserInRoles.After", "UserManagement", "UserService", "SaveUserInRoles", EventTypeEnum.after);

            #endregion Service Actions

            #endregion UserService

            #region UserMessageService

            #region Base Actions

            eventService.AddEvent ("UserMessageService.First.Before", "UserManagement", "UserMessageService", "First", EventTypeEnum.before);
            eventService.AddEvent ("UserMessageService.FirstOrDefault.Before", "UserManagement", "UserMessageService", "FirstOrDefault", EventTypeEnum.before);
            eventService.AddEvent ("UserMessageService.Any.Before", "UserManagement", "UserMessageService", "Any", EventTypeEnum.before);
            eventService.AddEvent ("UserMessageService.List.Before", "UserManagement", "UserMessageService", "List", EventTypeEnum.before);
            eventService.AddEvent ("UserMessageService.Count.Before", "UserManagement", "UserMessageService", "Count", EventTypeEnum.before);

            eventService.AddEvent ("UserMessageService.First.After", "UserManagement", "UserMessageService", "First", EventTypeEnum.after);
            eventService.AddEvent ("UserMessageService.FirstOrDefault.After", "UserManagement", "UserMessageService", "FirstOrDefault", EventTypeEnum.after);
            eventService.AddEvent ("UserMessageService.Any.After", "UserManagement", "UserMessageService", "Any", EventTypeEnum.after);
            eventService.AddEvent ("UserMessageService.List.After", "UserManagement", "UserMessageService", "List", EventTypeEnum.after);
            eventService.AddEvent ("UserMessageService.Count.After", "UserManagement", "UserMessageService", "Count", EventTypeEnum.after);

            #endregion Base Actions

            #region Service Actions

            eventService.AddEvent ("UserMessageService.SaveViewMessage.Before", "UserManagement", "UserMessageService", "SaveViewMessage", EventTypeEnum.before);

            eventService.AddEvent ("UserMessageService.SaveViewMessage.After", "UserManagement", "UserMessageService", "SaveViewMessage", EventTypeEnum.after);

            #endregion Service Actions

            #endregion UserMessageService

            #region TokenService

            #region Base Actions

            eventService.AddEvent ("RoleService.First.Before", "UserManagement", "RoleService", "First", EventTypeEnum.before);
            eventService.AddEvent ("RoleService.FirstOrDefault.Before", "UserManagement", "RoleService", "FirstOrDefault", EventTypeEnum.before);
            eventService.AddEvent ("RoleService.Any.Before", "UserManagement", "RoleService", "Any", EventTypeEnum.before);
            eventService.AddEvent ("RoleService.List.Before", "UserManagement", "RoleService", "List", EventTypeEnum.before);
            eventService.AddEvent ("RoleService.Count.Before", "UserManagement", "RoleService", "Count", EventTypeEnum.before);

            eventService.AddEvent ("RoleService.First.After", "UserManagement", "RoleService", "First", EventTypeEnum.after);
            eventService.AddEvent ("RoleService.FirstOrDefault.After", "UserManagement", "RoleService", "FirstOrDefault", EventTypeEnum.after);
            eventService.AddEvent ("RoleService.Any.After", "UserManagement", "RoleService", "Any", EventTypeEnum.after);
            eventService.AddEvent ("RoleService.List.After", "UserManagement", "RoleService", "List", EventTypeEnum.after);
            eventService.AddEvent ("RoleService.Count.After", "UserManagement", "RoleService", "Count", EventTypeEnum.after);

            #endregion Base Actions

            #endregion TokenService

            #region RoleService

            #region Base Actions

            eventService.AddEvent ("RoleService.First.Before", "UserManagement", "RoleService", "First", EventTypeEnum.before);
            eventService.AddEvent ("RoleService.FirstOrDefault.Before", "UserManagement", "RoleService", "FirstOrDefault", EventTypeEnum.before);
            eventService.AddEvent ("RoleService.Any.Before", "UserManagement", "RoleService", "Any", EventTypeEnum.before);
            eventService.AddEvent ("RoleService.List.Before", "UserManagement", "RoleService", "List", EventTypeEnum.before);
            eventService.AddEvent ("RoleService.Count.Before", "UserManagement", "RoleService", "Count", EventTypeEnum.before);

            eventService.AddEvent ("RoleService.First.After", "UserManagement", "RoleService", "First", EventTypeEnum.after);
            eventService.AddEvent ("RoleService.FirstOrDefault.After", "UserManagement", "RoleService", "FirstOrDefault", EventTypeEnum.after);
            eventService.AddEvent ("RoleService.Any.After", "UserManagement", "RoleService", "Any", EventTypeEnum.after);
            eventService.AddEvent ("RoleService.List.After", "UserManagement", "RoleService", "List", EventTypeEnum.after);
            eventService.AddEvent ("RoleService.Count.After", "UserManagement", "RoleService", "Count", EventTypeEnum.after);

            #endregion Base Actions

            #endregion RoleService

            #region GroupService

            #region Base Actions

            eventService.AddEvent ("GroupService.First.Before", "UserManagement", "GroupService", "First", EventTypeEnum.before);
            eventService.AddEvent ("GroupService.FirstOrDefault.Before", "UserManagement", "GroupService", "FirstOrDefault", EventTypeEnum.before);
            eventService.AddEvent ("GroupService.Any.Before", "UserManagement", "GroupService", "Any", EventTypeEnum.before);
            eventService.AddEvent ("GroupService.List.Before", "UserManagement", "GroupService", "List", EventTypeEnum.before);
            eventService.AddEvent ("GroupService.Count.Before", "UserManagement", "GroupService", "Count", EventTypeEnum.before);

            eventService.AddEvent ("GroupService.First.After", "UserManagement", "GroupService", "First", EventTypeEnum.after);
            eventService.AddEvent ("GroupService.FirstOrDefault.After", "UserManagement", "GroupService", "FirstOrDefault", EventTypeEnum.after);
            eventService.AddEvent ("GroupService.Any.After", "UserManagement", "GroupService", "Any", EventTypeEnum.after);
            eventService.AddEvent ("GroupService.List.After", "UserManagement", "GroupService", "List", EventTypeEnum.after);
            eventService.AddEvent ("GroupService.Count.After", "UserManagement", "GroupService", "Count", EventTypeEnum.after);

            #endregion Base Actions

            #region Service Actions

            eventService.AddEvent ("GroupService.ListWithDetails.Before", "UserManagement", "GroupService", "ListWithDetails", EventTypeEnum.before);
            eventService.AddEvent ("GroupService.Save.Before", "UserManagement", "GroupService", "Save", EventTypeEnum.before);
            eventService.AddEvent ("GroupService.ChangeStatus.Before", "UserManagement", "GroupService", "ChangeStatus", EventTypeEnum.before);
            eventService.AddEvent ("GroupService.SaveGroupInRoles.Before", "UserManagement", "GroupService", "SaveGroupInRoles", EventTypeEnum.before);

            eventService.AddEvent ("GroupService.ListWithDetails.After", "UserManagement", "GroupService", "ListWithDetails", EventTypeEnum.after);
            eventService.AddEvent ("GroupService.Save.After", "UserManagement", "GroupService", "Save", EventTypeEnum.after);
            eventService.AddEvent ("GroupService.ChangeStatus.After", "UserManagement", "GroupService", "ChangeStatus", EventTypeEnum.after);
            eventService.AddEvent ("GroupService.SaveGroupInRoles.After", "UserManagement", "GroupService", "SaveGroupInRoles", EventTypeEnum.after);

            #endregion Service Actions

            #endregion GroupService
        }
    }
}