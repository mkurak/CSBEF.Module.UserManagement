using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Core.Models.HubModels;
using CSBEF.Module.UserManagement.Enums.Errors;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Module.UserManagement.Poco;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CSBEF.Module.UserManagement.Services {
    public class GroupService : ServiceBase<Group, GroupDTO>, IGroupService {
        #region Dependencies

        private readonly IGroupInRoleRepository _groupInRoleRepository;
        private readonly IUserInGroupRepository _userInGroupRepository;
        private readonly ITokenRepository _tokenRepository;

        #endregion Dependencies

        #region ctor

        public GroupService (
            IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration,
            ILogger<IReturnModel<bool>> logger,
            IMapper mapper,
            IGroupRepository repository,
            IEventService eventService,

            // Other Repository Dependencies
            IGroupInRoleRepository groupInRoleRepository,
            IUserInGroupRepository userInGroupRepository,
            ITokenRepository tokenRepository,
            IHubSyncDataService hubSyncDataService
        ) : base (
            hostingEnvironment,
            configuration,
            logger,
            mapper,
            repository,
            eventService,
            hubSyncDataService,
            "UserManagement",
            "GroupService"
        ) {
            _groupInRoleRepository = groupInRoleRepository;
            _userInGroupRepository = userInGroupRepository;
            _tokenRepository = tokenRepository;
        }

        #endregion ctor

        #region Public Actions

        public IReturnModel<IList<UserGroupDetailsModel>> ListWithDetails (ServiceParamsWithIdentifier<ActionFilterModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<IList<UserGroupDetailsModel>> rtn = new ReturnModel<IList<UserGroupDetailsModel>> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<IList<UserGroupDetailsModel>>, ServiceParamsWithIdentifier<ActionFilterModel>> afterEventParameterModel = null;
                IReturnModel<IList<UserGroupDetailsModel>> afterEventHandler = null;
                IReturnModel<IList<GroupDTO>> getList = null;
                UserGroupDetailsModel newItem = null;
                ICollection<GroupInRole> getRoles = null;
                ICollection<UserInGroup> getUsers = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ListWithDetails.Before").EventHandler<bool, ServiceParamsWithIdentifier<ActionFilterModel>> (args);
                if (beforeEventHandler != null) {
                    if (beforeEventHandler.ErrorInfo.Status) {
                        rtn.ErrorInfo = beforeEventHandler.ErrorInfo;
                        cnt = false;
                    }
                }

                #endregion Before Event Handler

                #region Action Body

                if (cnt) {
                    rtn.Result = new List<UserGroupDetailsModel> ();

                    getList = base.List (args.Param);
                    if (getList.ErrorInfo.Status) {
                        rtn.ErrorInfo = getList.ErrorInfo;
                        cnt = false;
                    }
                }

                if (cnt) {
                    if (!getList.Result.Any ())
                        cnt = false;
                }

                if (cnt) {
                    foreach (var item in getList.Result) {
                        newItem = new UserGroupDetailsModel ();
                        newItem = Mapper.Map<UserGroupDetailsModel> (item);

                        getRoles = _groupInRoleRepository.FindAll (i => i.GroupId == item.Id);
                        if (getRoles.Any ()) {
                            foreach (var inRole in getRoles)
                                newItem.Roles.Add (inRole.RoleId);
                        }

                        getUsers = _userInGroupRepository.FindAll (i => i.GroupId == item.Id);
                        if (getUsers.Any ()) {
                            foreach (var user in getUsers) {
                                if (!newItem.Users.Any (i => i == user.UserId))
                                    newItem.Users.Add (user.UserId);
                            }
                        }

                        rtn.Result.Add (newItem);
                    }
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<IList<UserGroupDetailsModel>>, ServiceParamsWithIdentifier<ActionFilterModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ListWithDetails"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ListWithDetails.After")
                        .EventHandler<IList<UserGroupDetailsModel>, IAfterEventParameterModel<IReturnModel<IList<UserGroupDetailsModel>>, ServiceParamsWithIdentifier<ActionFilterModel>>> (afterEventParameterModel);
                    if (afterEventHandler != null) {
                        if (afterEventHandler.ErrorInfo.Status) {
                            rtn.ErrorInfo = afterEventHandler.ErrorInfo;
                            cnt = false;
                        } else {
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
                getList = null;
                newItem = null;
                getRoles = null;
                getUsers = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<GroupDTO> Save (ServiceParamsWithIdentifier<SaveGroupModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<GroupDTO> rtn = new ReturnModel<GroupDTO> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<GroupDTO>, ServiceParamsWithIdentifier<SaveGroupModel>> afterEventParameterModel = null;
                IReturnModel<GroupDTO> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                Group getData = null;
                Group checkName = null;
                HubSyncDataModel<UserGroupDetailsModel> syncDataModel = null;
                ICollection<GroupInRole> roles = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.Save.Before").EventHandler<bool, ServiceParamsWithIdentifier<SaveGroupModel>> (args);
                if (beforeEventHandler != null) {
                    if (beforeEventHandler.ErrorInfo.Status) {
                        rtn.ErrorInfo = beforeEventHandler.ErrorInfo;
                        cnt = false;
                    }
                }

                #endregion Before Event Handler

                #region Action Body

                if (cnt) {
                    modelValidation = args.Param.ModelValidation ();

                    if (modelValidation.Any ()) {
                        rtn = rtn.SendError (GroupErrorsEnum.SaveModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt && args.Param.Id > 0) {
                    getData = Repository.Find (i => i.Id == args.Param.Id);
                    if (getData == null) {
                        rtn = rtn.SendError (GroupErrorsEnum.SaveDataNotFound);
                        cnt = false;
                    }
                }

                if (cnt) {
                    if (args.Param.Id == 0) {
                        checkName = Repository.Find (i => i.GroupName == args.Param.GroupName);
                        if (checkName != null) {
                            rtn = rtn.SendError (GroupErrorsEnum.SaveGroupNameExists);
                            cnt = false;
                        }
                    } else {
                        checkName = Repository.Find (i => i.GroupName == args.Param.GroupName && i.Id != args.Param.Id);
                        if (checkName != null) {
                            rtn = rtn.SendError (GroupErrorsEnum.SaveGroupNameExists);
                            cnt = false;
                        }
                    }
                }

                if (cnt) {
                    if (getData == null) {
                        getData = new Group {
                        GroupName = args.Param.GroupName,
                        Status = true,
                        AddingDate = DateTime.Now,
                        AddingUserId = args.UserId,
                        UpdatingDate = DateTime.Now,
                        UpdatingUserId = args.UserId
                        };
                        getData = Repository.Add (getData);
                    } else {
                        getData.GroupName = args.Param.GroupName;
                        getData.UpdatingDate = DateTime.Now;
                        getData.UpdatingUserId = args.UserId;
                        getData = Repository.Update (getData);
                    }

                    Repository.Save ();

                    rtn.Result = Mapper.Map<GroupDTO> (getData);
                }

                if (cnt) {
                    syncDataModel = new HubSyncDataModel<UserGroupDetailsModel> {
                        Key = "UserManagement_UserGroup",
                        ProcessType = args.Param.Id > 0 ? "update" : "add",
                        Id = args.Param.Id,
                        UserId = args.UserId,
                        Name = getData.GroupName,
                        Data = new UserGroupDetailsModel ()
                    };

                    syncDataModel.Data = Mapper.Map<UserGroupDetailsModel> (rtn.Result);

                    roles = _groupInRoleRepository.FindAll (i => i.GroupId == syncDataModel.Data.Id);
                    if (roles.Any ())
                        syncDataModel.Data.Roles.AddRange (roles.Select (i => i.RoleId));

                    var users = _userInGroupRepository.FindAll (i => i.GroupId == syncDataModel.Data.Id);
                    if (users.Any ())
                        syncDataModel.Data.Users.AddRange (users.Select (i => i.UserId));

                    HubSyncDataService.OnSync (syncDataModel);
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<GroupDTO>, ServiceParamsWithIdentifier<SaveGroupModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "Save"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.Save.After")
                        .EventHandler<GroupDTO, IAfterEventParameterModel<IReturnModel<GroupDTO>, ServiceParamsWithIdentifier<SaveGroupModel>>> (afterEventParameterModel);
                    if (afterEventHandler != null) {
                        if (afterEventHandler.ErrorInfo.Status) {
                            rtn.ErrorInfo = afterEventHandler.ErrorInfo;
                            cnt = false;
                        } else {
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
                modelValidation = null;
                getData = null;
                checkName = null;
                syncDataModel = null;
                roles = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<bool> ChangeStatus (ServiceParamsWithIdentifier<ChangeStatusModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<bool> rtn = new ReturnModel<bool> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeStatusModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                Group getData = null;
                ICollection<Token> tokens = null;
                ICollection<UserInGroup> getUsers = null;
                HubSyncDataModel<bool> syncDataModel = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangeStatus.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangeStatusModel>> (args);
                if (beforeEventHandler != null) {
                    if (beforeEventHandler.ErrorInfo.Status) {
                        rtn.ErrorInfo = beforeEventHandler.ErrorInfo;
                        cnt = false;
                    }
                }

                #endregion Before Event Handler

                #region Action Body

                if (cnt) {
                    modelValidation = args.Param.ModelValidation ();

                    if (modelValidation.Any ()) {
                        rtn = rtn.SendError (UserErrorsEnum.ChangeStatusModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt) {
                    getData = Repository.Find (i => i.Id == args.Param.Id);
                    if (getData == null) {
                        rtn = rtn.SendError (UserErrorsEnum.ChangeStatusDataNotFound);
                        cnt = false;
                    }
                }

                if (cnt) {
                    getData.Status = args.Param.Status;
                    getData.UpdatingDate = DateTime.Now;
                    getData.UpdatingUserId = args.UserId;
                    getData = Repository.Update (getData);
                    Repository.Save ();
                }

                if (cnt) {
                    getUsers = _userInGroupRepository.FindAll (i => i.GroupId == args.Param.Id);
                    if (getUsers.Any ()) {
                        foreach (var user in getUsers) {
                            tokens = _tokenRepository.FindAll (i => i.UserId == user.Id);
                            if (tokens.Any ()) {
                                foreach (var token in tokens) {
                                    token.Status = false;
                                    token.UpdatingDate = DateTime.Now;
                                    token.UpdatingUserId = args.UserId;
                                    _tokenRepository.Update (token);
                                }
                                _tokenRepository.Save ();
                            }
                        }
                    }
                }

                if (cnt) {
                    if (!args.Param.Status) {
                        var relationUsers = _userInGroupRepository.FindAll (i => i.GroupId == args.Param.Id);
                        if (relationUsers.Any ()) {
                            foreach (var item in relationUsers) {
                                _userInGroupRepository.Delete (item);
                                var getTokens = _tokenRepository.FindAll (i => i.UserId == item.UserId);
                                if (getTokens.Any ()) {
                                    foreach (var token in getTokens) {
                                        token.Status = false;
                                        token.UpdatingDate = DateTime.Now;
                                        token.UpdatingUserId = args.UserId;
                                        _tokenRepository.Update (token);
                                    }
                                }
                            }
                            _userInGroupRepository.Save ();
                        }
                    }
                }

                if (cnt) {
                    syncDataModel = new HubSyncDataModel<bool> {
                        Key = "UserManagement_UserGroup",
                        ProcessType = "remove",
                        Id = args.Param.Id,
                        UserId = args.UserId,
                        Name = getData.GroupName,
                        Data = args.Param.Status
                    };

                    HubSyncDataService.OnSync (syncDataModel);
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeStatusModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ChangeStatus"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangeStatus.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeStatusModel>>> (afterEventParameterModel);
                    if (afterEventHandler != null) {
                        if (afterEventHandler.ErrorInfo.Status) {
                            rtn.ErrorInfo = afterEventHandler.ErrorInfo;
                            cnt = false;
                        } else {
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
                modelValidation = null;
                getData = null;
                tokens = null;
                getUsers = null;
                syncDataModel = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<bool> SaveGroupInRoles (ServiceParamsWithIdentifier<SaveGroupInRoleModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<bool> rtn = new ReturnModel<bool> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveGroupInRoleModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                ICollection<GroupInRole> inRoleRecords = null;
                string[] rolesArray = null;
                ICollection<UserInGroup> findUsers = null;
                ICollection<Token> findTokens = null;
                HubSyncDataModel<UserGroupDetailsModel> syncDataModel = null;
                ICollection<GroupInRole> roles = null;
                Group getData = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.SaveGroupInRoles.Before").EventHandler<bool, ServiceParamsWithIdentifier<SaveGroupInRoleModel>> (args);
                if (beforeEventHandler != null) {
                    if (beforeEventHandler.ErrorInfo.Status) {
                        rtn.ErrorInfo = beforeEventHandler.ErrorInfo;
                        cnt = false;
                    }
                }

                #endregion Before Event Handler

                #region Action Body

                if (cnt) {
                    modelValidation = args.Param.ModelValidation ();

                    if (modelValidation.Any ()) {
                        rtn = rtn.SendError (GroupErrorsEnum.SaveGroupInRolesModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt) {
                    inRoleRecords = _groupInRoleRepository.FindAll (i => i.GroupId == args.Param.GroupId);
                    if (inRoleRecords.Any ()) {
                        foreach (var data in inRoleRecords)
                            _groupInRoleRepository.Delete (data);

                        _groupInRoleRepository.Save ();
                    }

                    if (!string.IsNullOrWhiteSpace (args.Param.Roles)) {
                        rolesArray = args.Param.Roles.Split (",");
                        if (rolesArray.Length > 0) {
                            foreach (var data in rolesArray) {
                                if (data.ToInt (0) > 0) {
                                    _groupInRoleRepository.Add (new GroupInRole {
                                        GroupId = args.Param.GroupId,
                                            RoleId = data.ToInt (0),
                                            Status = true,
                                            AddingDate = DateTime.Now,
                                            AddingUserId = args.UserId,
                                            UpdatingDate = DateTime.Now,
                                            UpdatingUserId = args.UserId
                                    });
                                }
                            }

                            _groupInRoleRepository.Save ();
                        }
                    }

                    findUsers = _userInGroupRepository.FindAll (i => i.GroupId == args.Param.GroupId);
                    if (findUsers.Any ()) {
                        foreach (var user in findUsers) {
                            findTokens = _tokenRepository.FindAll (i => i.UserId == user.Id && i.Status == true);
                            if (findTokens.Any ()) {
                                foreach (var token in findTokens) {
                                    token.Status = false;
                                    token.UpdatingDate = DateTime.Now;
                                    token.UpdatingUserId = args.UserId;
                                    _tokenRepository.Update (token);
                                }

                                _tokenRepository.Save ();
                            }
                        }
                    }
                }

                if (cnt) {
                    getData = Repository.Find (i => i.Id == args.Param.GroupId);

                    syncDataModel = new HubSyncDataModel<UserGroupDetailsModel> {
                        Key = "UserManagement_UserGroup",
                        ProcessType = "changeRoles",
                        Id = args.Param.GroupId,
                        UserId = args.UserId,
                        Name = getData.GroupName,
                        Data = new UserGroupDetailsModel ()
                    };

                    syncDataModel.Data = Mapper.Map<UserGroupDetailsModel> (Mapper.Map<GroupDTO> (getData));

                    roles = _groupInRoleRepository.FindAll (i => i.GroupId == syncDataModel.Data.Id);
                    if (roles.Any ())
                        syncDataModel.Data.Roles.AddRange (roles.Select (i => i.RoleId));

                    var users = _userInGroupRepository.FindAll (i => i.GroupId == syncDataModel.Data.Id);
                    if (users.Any ())
                        syncDataModel.Data.Users.AddRange (users.Select (i => i.UserId));

                    HubSyncDataService.OnSync (syncDataModel);
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveGroupInRoleModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "SaveGroupInRoles"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.SaveGroupInRoles.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveGroupInRoleModel>>> (afterEventParameterModel);
                    if (afterEventHandler != null) {
                        if (afterEventHandler.ErrorInfo.Status) {
                            rtn.ErrorInfo = afterEventHandler.ErrorInfo;
                            cnt = false;
                        } else {
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
                modelValidation = null;
                inRoleRecords = null;
                rolesArray = null;
                findUsers = null;
                findTokens = null;
                syncDataModel = null;
                roles = null;
                getData = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        #endregion Public Actions
    }
}