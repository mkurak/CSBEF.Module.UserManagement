using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using AutoMapper;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Core.Models.HelperModels;
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
    public class UserService : ServiceBase<User, UserDTO>, IUserService {
        #region Dependencies

        private readonly ITokenRepository _tokenRepository;
        private readonly IUserInGroupRepository _userInGroupRepository;
        private readonly IGroupInRoleRepository _groupInRoleRepository;
        private readonly IUserInRoleRepository _userInRoleRepository;

        #endregion Dependencies

        #region ctor

        public UserService (
            IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration,
            ILogger<IReturnModel<bool>> logger,
            IMapper mapper,
            IUserRepository repository,
            IEventService eventService,

            // Other Repository Dependencies
            ITokenRepository tokenRepository,
            IUserInGroupRepository userInGroupRepository,
            IGroupInRoleRepository groupInRoleRepository,
            IUserInRoleRepository userInRoleRepository,
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
            "UserService"
        ) {
            _tokenRepository = tokenRepository;
            _userInGroupRepository = userInGroupRepository;
            _groupInRoleRepository = groupInRoleRepository;
            _userInRoleRepository = userInRoleRepository;
        }

        #endregion ctor

        #region Public Actions

        public IReturnModel<bool> ChangePassword (ServiceParamsWithIdentifier<ChangePasswordModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<bool> rtn = new ReturnModel<bool> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangePasswordModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                User getUser = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangePassword.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangePasswordModel>> (args);
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
                        rtn = rtn.SendError (UserErrorsEnum.ChangePasswordModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt) {
                    getUser = Repository.Find (i => i.Id == args.Param.UserId);
                    if (getUser == null) {
                        rtn = rtn.SendError (UserErrorsEnum.ChangePasswordUserNotFound);
                        cnt = false;
                    }
                }

                if (cnt) {
                    if (getUser.Password.ToUpper (Thread.CurrentThread.CurrentCulture) != Tools.ComputeSha256Hash (args.Param.CurrentPass).ToUpper (Thread.CurrentThread.CurrentCulture)) {
                        rtn = rtn.SendError (UserErrorsEnum.ChangePasswordWrongCurrentPassword);
                        cnt = false;
                    }
                }

                if (cnt) {
                    getUser.Password = Tools.ComputeSha256Hash (args.Param.NewPass).ToUpper (Thread.CurrentThread.CurrentCulture);
                    Repository.Update (getUser);
                    Repository.Save ();
                }

                if (cnt) {
                    _tokenRepository.KillUserTokens (args.Param.UserId, args.UserId);
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangePasswordModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ChangePassword"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangePassword.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangePasswordModel>>> (afterEventParameterModel);
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
                getUser = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<string> ChangePicture (ServiceParamsWithIdentifier<ChangePictureModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<string> rtn = new ReturnModel<string> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>> afterEventParameterModel = null;
                IReturnModel<string> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                User getUser = null;
                string uploaderRootPath = null;
                FileUploader newFileUpload = null;
                IReturnModel<string> savedFileName = null;
                string targetRemoveFile = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangePicture.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangePictureModel>> (args);
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
                        rtn = rtn.SendError (UserErrorsEnum.ChangePictureModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt) {
                    getUser = Repository.Find (i => i.Id == args.Param.Id);
                    if (getUser == null) {
                        rtn = rtn.SendError (UserErrorsEnum.ChangePictureUserNotFound);
                        cnt = false;
                    }
                }

                if (cnt) {
                    uploaderRootPath = Path.Combine (GlobalConfiguration.SWwwRootPath, Configuration["AppSettings:FileUploader:UserManagement:ProfilePicRootPath"]);
                    newFileUpload = new FileUploader (Configuration, Logger, null, 0, 0, uploaderRootPath);
                    savedFileName = newFileUpload.Upload (args.Param.Picture);
                    if (savedFileName.ErrorInfo.Status) {
                        rtn.ErrorInfo = savedFileName.ErrorInfo;
                        cnt = false;
                    }
                }

                if (cnt) {
                    if (!string.IsNullOrWhiteSpace (getUser.ProfilePic)) {
                        if (getUser.ProfilePic != "default.jpg") {
                            targetRemoveFile = Path.Combine (uploaderRootPath, getUser.ProfilePic);
                            File.Delete (targetRemoveFile);
                        }
                    }

                    getUser.ProfilePic = savedFileName.Result;
                    Repository.Update (getUser);
                    Repository.Save ();

                    rtn.Result = getUser.ProfilePic;
                }

                if (cnt) {
                    HubSyncDataService.OnSync (new HubSyncDataModel<UserDetailsModel> {
                        Key = "UserManagement_User",
                        ProcessType = "update",
                        Id = args.Param.Id,
                        UserId = args.UserId,
                        Name = getUser.Name + " " + getUser.Surname,
                        Data = GetUserDetails (args.Param.Id).Result
                    });
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ChangePicture"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangePicture.After")
                        .EventHandler<string, IAfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>>> (afterEventParameterModel);
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
                getUser = null;
                uploaderRootPath = null;
                newFileUpload = null;
                savedFileName = null;
                targetRemoveFile = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<string> ChangeBgPicture (ServiceParamsWithIdentifier<ChangePictureModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<string> rtn = new ReturnModel<string> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>> afterEventParameterModel = null;
                IReturnModel<string> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                User getUser = null;
                string uploaderRootPath = null;
                FileUploader newFileUpload = null;
                IReturnModel<string> savedFileName = null;
                string targetRemoveFile = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangeBgPicture.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangePictureModel>> (args);
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
                        rtn = rtn.SendError (UserErrorsEnum.ChangePictureModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt) {
                    getUser = Repository.Find (i => i.Id == args.Param.Id);
                    if (getUser == null) {
                        rtn = rtn.SendError (UserErrorsEnum.ChangePictureUserNotFound);
                        cnt = false;
                    }
                }

                if (cnt) {
                    uploaderRootPath = Path.Combine (GlobalConfiguration.SWwwRootPath, Configuration["AppSettings:FileUploader:UserManagement:ProfileBgPicRootPath"]);
                    newFileUpload = new FileUploader (Configuration, Logger, null, 0, 0, uploaderRootPath);
                    savedFileName = newFileUpload.Upload (args.Param.Picture);
                    if (savedFileName.ErrorInfo.Status) {
                        rtn.ErrorInfo = savedFileName.ErrorInfo;
                        cnt = false;
                    }
                }

                if (cnt) {
                    if (!string.IsNullOrWhiteSpace (getUser.ProfileBgPic)) {
                        if (getUser.ProfileBgPic != "default.jpg") {
                            targetRemoveFile = Path.Combine (uploaderRootPath, getUser.ProfilePic);
                            File.Delete (targetRemoveFile);
                        }
                    }

                    getUser.ProfileBgPic = savedFileName.Result;
                    Repository.Update (getUser);
                    Repository.Save ();

                    rtn.Result = getUser.ProfilePic;
                }

                if (cnt) {
                    HubSyncDataService.OnSync (new HubSyncDataModel<UserDetailsModel> {
                        Key = "UserManagement_User",
                        ProcessType = "update",
                        Id = args.Param.Id,
                        UserId = args.UserId,
                        Name = getUser.Name + " " + getUser.Surname,
                        Data = (GetUserDetails (args.Param.Id)).Result
                    });
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ChangeBgPicture"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangeBgPicture.After")
                        .EventHandler<string, IAfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>>> (afterEventParameterModel);
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
                getUser = null;
                uploaderRootPath = null;
                newFileUpload = null;
                savedFileName = null;
                targetRemoveFile = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<bool> ChangeProfileInformations (ServiceParamsWithIdentifier<ChangeProfileInformationsModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<bool> rtn = new ReturnModel<bool> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeProfileInformationsModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                User getData = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangeProfileInformations.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangeProfileInformationsModel>> (args);
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
                        rtn = rtn.SendError (UserErrorsEnum.ChangeProfileInformationsModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt) {
                    getData = Repository.Find (i => i.Id == args.Param.Id);
                    if (getData == null) {
                        rtn = rtn.SendError (UserErrorsEnum.ChangeProfileInformationsDataFound);
                        cnt = false;
                    }
                }

                if (cnt) {
                    getData.Name = args.Param.Name;
                    getData.Surname = args.Param.Surname;
                    getData.ProfileStatusMessage = args.Param.ProfileStatusMessage;
                    getData.UpdatingDate = DateTime.Now;
                    getData.UpdatingUserId = args.UserId;
                    Repository.Update (getData);
                    Repository.Save ();
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeProfileInformationsModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "Add"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ChangeProfileInformations.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeProfileInformationsModel>>> (afterEventParameterModel);
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

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<IList<UserDetailsModel>> ListWithDetails (ServiceParamsWithIdentifier<ActionFilterModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<IList<UserDetailsModel>> rtn = new ReturnModel<IList<UserDetailsModel>> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<IList<UserDetailsModel>>, ServiceParamsWithIdentifier<ActionFilterModel>> afterEventParameterModel = null;
                IReturnModel<IList<UserDetailsModel>> afterEventHandler = null;
                IReturnModel<IList<UserDTO>> baseList = null;
                IReturnModel<UserDetailsModel> getUserDetails = null;

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
                    rtn.Result = new List<UserDetailsModel> ();

                    baseList = base.List (args.Param);
                    if (!baseList.Result.Any ())
                        cnt = false;
                }

                if (cnt) {
                    foreach (var user in baseList.Result) {
                        getUserDetails = GetUserDetails (user.Id);
                        if (getUserDetails.ErrorInfo.Status) {
                            rtn.ErrorInfo = getUserDetails.ErrorInfo;
                            cnt = false;
                        } else {
                            if (getUserDetails.Result != null)
                                rtn.Result.Add (getUserDetails.Result);
                        }
                    }
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<IList<UserDetailsModel>>, ServiceParamsWithIdentifier<ActionFilterModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ListWithDetails"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.ListWithDetails.After")
                        .EventHandler<IList<UserDetailsModel>, IAfterEventParameterModel<IReturnModel<IList<UserDetailsModel>>, ServiceParamsWithIdentifier<ActionFilterModel>>> (afterEventParameterModel);
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
                baseList = null;
                getUserDetails = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<UserDTO> Save (ServiceParamsWithIdentifier<SaveUserModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<UserDTO> rtn = new ReturnModel<UserDTO> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<UserDTO>, ServiceParamsWithIdentifier<SaveUserModel>> afterEventParameterModel = null;
                IReturnModel<UserDTO> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                User checkEmail = null;
                User checkUserName = null;
                User getData = null;
                User saveData = null;
                User savedData = null;
                ICollection<Token> getTokens = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.Save.Before").EventHandler<bool, ServiceParamsWithIdentifier<SaveUserModel>> (args);
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
                        rtn = rtn.SendError (UserErrorsEnum.SaveModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt) {
                    if (args.Param.Id > 0)
                        checkEmail = Repository.Find (i => i.Email == args.Param.Email && i.Id != args.Param.Id);
                    else
                        checkEmail = Repository.Find (i => i.Email == args.Param.Email);

                    if (checkEmail != null) {
                        rtn = rtn.SendError (UserErrorsEnum.SaveEmailExists);
                        cnt = false;
                    }
                }

                if (cnt) {
                    if (args.Param.Id > 0)
                        checkUserName = Repository.Find (i => i.UserName == args.Param.UserName && i.Id != args.Param.Id);
                    else
                        checkUserName = Repository.Find (i => i.UserName == args.Param.UserName);

                    if (checkUserName != null) {
                        rtn = rtn.SendError (UserErrorsEnum.SaveUserNameExists);
                        cnt = false;
                    }
                }

                if (cnt && args.Param.Id > 0) {
                    getData = Repository.Find (i => i.Id == args.Param.Id);
                    if (getData == null) {
                        rtn = rtn.SendError (UserErrorsEnum.SaveUserNotFound);
                        cnt = false;
                    }
                }

                if (cnt && args.Param.Id > 0) {
                    getData.Name = args.Param.Name;
                    getData.Surname = args.Param.Surname;
                    getData.UserName = args.Param.UserName;
                    getData.Email = args.Param.Email;
                    getData.UpdatingDate = DateTime.Now;
                    getData.UpdatingUserId = args.UserId;
                }

                if (cnt && args.Param.Id > 0 && !string.IsNullOrWhiteSpace (args.Param.CurrentPassword)) {
                    if (getData.Password.ToUpper (Thread.CurrentThread.CurrentCulture) != Tools.ComputeSha256Hash (args.Param.CurrentPassword).ToUpper (Thread.CurrentThread.CurrentCulture)) {
                        rtn = rtn.SendError (UserErrorsEnum.SaveCurrentPasswordWrong);
                        cnt = false;
                    } else {
                        getData.Password = Tools.ComputeSha256Hash (args.Param.NewPassword).ToUpper (Thread.CurrentThread.CurrentCulture);
                    }
                }

                if (cnt && args.Param.Id == 0 && string.IsNullOrWhiteSpace (args.Param.NewPassword)) {
                    rtn = rtn.SendError (UserErrorsEnum.SavePasswordRequired);
                    cnt = false;
                }

                if (cnt && args.Param.Id == 0) {
                    saveData = new User {
                    Name = args.Param.Name,
                    Surname = args.Param.Surname,
                    UserName = args.Param.UserName,
                    Email = args.Param.Email,
                    Password = Tools.ComputeSha256Hash (args.Param.NewPassword).ToUpper (Thread.CurrentThread.CurrentCulture),
                    ProfilePic = "default.jpg",
                    ProfileBgPic = "default.jpg",
                    ProfileStatusMessage = "Hello Worl!",
                    AddingDate = DateTime.Now,
                    UpdatingDate = DateTime.Now,
                    AddingUserId = args.UserId,
                    UpdatingUserId = args.UserId
                    };

                    savedData = Repository.Add (saveData);
                    Repository.Save ();
                    rtn.Result = Mapper.Map<UserDTO> (savedData);
                }

                if (cnt && args.Param.Id > 0) {
                    savedData = Repository.Update (getData);
                    Repository.Save ();
                    rtn.Result = Mapper.Map<UserDTO> (savedData);

                    getTokens = _tokenRepository.FindAll (i => i.UserId == args.Param.Id);
                    if (getTokens.Any ()) {
                        foreach (var token in getTokens) {
                            token.Status = false;
                            token.UpdatingDate = DateTime.Now;
                            token.UpdatingUserId = args.UserId;
                            _tokenRepository.Update (token);
                        }

                        Repository.Save ();
                    }
                }

                if (cnt) {
                    HubSyncDataService.OnSync (new HubSyncDataModel<UserDetailsModel> {
                        Key = "UserManagement_User",
                        ProcessType = args.Param.Id > 0 ? "update" : "add",
                        Id = args.Param.Id.ToInt (0),
                        UserId = args.UserId,
                        Name = savedData.Name + " " + savedData.Surname,
                        Data = GetUserDetails (savedData.Id).Result
                    });
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<UserDTO>, ServiceParamsWithIdentifier<SaveUserModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "Save"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.Save.After")
                        .EventHandler<UserDTO, IAfterEventParameterModel<IReturnModel<UserDTO>, ServiceParamsWithIdentifier<SaveUserModel>>> (afterEventParameterModel);
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
                checkEmail = null;
                checkUserName = null;
                getData = null;
                saveData = null;
                savedData = null;
                getTokens = null;

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
                User getData = null;
                ICollection<Token> tokens = null;

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
                    tokens = _tokenRepository.FindAll (i => i.UserId == args.Param.Id);
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

                if (cnt) {
                    HubSyncDataService.OnSync (new HubSyncDataModel<int> {
                        Key = "UserManagement_User",
                        ProcessType = "remove",
                        Id = args.Param.Id,
                        UserId = args.UserId,
                        Name = getData.Name + " " + getData.Surname,
                        Data = args.Param.Id
                    });
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

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<bool> SaveUserInGroups (ServiceParamsWithIdentifier<SaveUserInGroupsModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<bool> rtn = new ReturnModel<bool> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInGroupsModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                ICollection<UserInGroup> items = null;
                string[] groupsArray = null;
                ICollection<Token> tokens = null;
                IReturnModel<UserDetailsModel> userDetails = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.SaveUserInGroups.Before").EventHandler<bool, ServiceParamsWithIdentifier<SaveUserInGroupsModel>> (args);
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
                        rtn = rtn.SendError (UserErrorsEnum.SaveUserInGroupsModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt) {
                    items = _userInGroupRepository.FindAll (i => i.UserId == args.Param.UserId);
                    if (items.Any ()) {
                        foreach (var item in items) {
                            _userInGroupRepository.Delete (item);
                        }

                        _userInGroupRepository.Save ();
                    }

                    groupsArray = args.Param.Groups.Split (",");

                    if (groupsArray.Any ()) {
                        foreach (var group in groupsArray) {
                            var convertIntGroup = group.ToInt (0);
                            if (convertIntGroup > 0) {
                                _userInGroupRepository.Add (new UserInGroup {
                                    UserId = args.Param.UserId,
                                        GroupId = convertIntGroup,
                                        Status = true,
                                        AddingDate = DateTime.Now,
                                        UpdatingDate = DateTime.Now,
                                        AddingUserId = args.UserId,
                                        UpdatingUserId = args.UserId
                                });
                            }
                        }

                        _userInGroupRepository.Save ();
                    }

                    tokens = _tokenRepository.FindAll (i => i.UserId == args.Param.UserId);
                    if (tokens.Any ()) {
                        foreach (var token in tokens) {
                            token.Status = false;
                            token.UpdatingDate = DateTime.Now;
                            token.UpdatingUserId = args.UserId;
                            _tokenRepository.Update (token);
                        }

                        _userInGroupRepository.Save ();
                    }
                }

                if (cnt) {
                    userDetails = GetUserDetails (args.Param.UserId);

                    HubSyncDataService.OnSync (new HubSyncDataModel<UserDetailsModel> {
                        Key = "UserManagement_User",
                        ProcessType = "update",
                        Id = args.Param.UserId,
                        UserId = args.UserId,
                        Name = userDetails.Result.User.Name + " " + userDetails.Result.User.Surname,
                        Data = userDetails.Result
                    });
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInGroupsModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "SaveUserInGroups"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.SaveUserInGroups.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInGroupsModel>>> (afterEventParameterModel);
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
                items = null;
                tokens = null;
                userDetails = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        public IReturnModel<bool> SaveUserInRoles (ServiceParamsWithIdentifier<SaveUserInRolesModel> args) {
            if (args == null)
                throw new ArgumentNullException (nameof (args));

            IReturnModel<bool> rtn = new ReturnModel<bool> (Logger);

            try {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInRolesModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                ICollection<UserInRole> items = null;
                string[] rolesArray = null;
                ICollection<Token> tokens = null;
                IReturnModel<UserDetailsModel> userDetails = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.SaveUserInRoles.Before").EventHandler<bool, ServiceParamsWithIdentifier<SaveUserInRolesModel>> (args);
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
                        rtn = rtn.SendError (UserErrorsEnum.SaveUserInRolesModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt) {
                    items = _userInRoleRepository.FindAll (i => i.UserId == args.Param.UserId);
                    if (items.Any ()) {
                        foreach (var item in items) {
                            _userInRoleRepository.Delete (item);
                        }

                        _userInRoleRepository.Save ();
                    }

                    rolesArray = args.Param.Roles.Split (",");

                    if (rolesArray.Any ()) {
                        foreach (var role in rolesArray) {
                            var convertIntrole = role.ToInt (0);
                            if (convertIntrole > 0) {
                                _userInRoleRepository.Add (new UserInRole {
                                    UserId = args.Param.UserId,
                                        RoleId = convertIntrole,
                                        Status = true,
                                        AddingDate = DateTime.Now,
                                        UpdatingDate = DateTime.Now,
                                        AddingUserId = args.UserId,
                                        UpdatingUserId = args.UserId
                                });
                            }
                        }

                        _userInRoleRepository.Save ();
                    }

                    tokens = _tokenRepository.FindAll (i => i.UserId == args.Param.UserId);
                    if (tokens.Any ()) {
                        foreach (var token in tokens) {
                            token.Status = false;
                            token.UpdatingDate = DateTime.Now;
                            token.UpdatingUserId = args.UserId;
                            _tokenRepository.Update (token);
                        }

                        _userInGroupRepository.Save ();
                    }
                }

                if (cnt) {
                    userDetails = GetUserDetails (args.Param.UserId);

                    HubSyncDataService.OnSync (new HubSyncDataModel<UserDetailsModel> {
                        Key = "UserManagement_User",
                        ProcessType = "update",
                        Id = args.Param.UserId,
                        UserId = args.UserId,
                        Name = userDetails.Result.User.Name + " " + userDetails.Result.User.Surname,
                        Data = userDetails.Result
                    });
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt) {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInRolesModel>> {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "SaveUserInRoles"
                    };
                    afterEventHandler = EventService.GetEvent (ModuleName, $"{ServiceName}.SaveUserInRoles.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInRolesModel>>> (afterEventParameterModel);
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
                items = null;
                tokens = null;
                userDetails = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        private IReturnModel<UserDetailsModel> GetUserDetails (int id) {
            IReturnModel<UserDetailsModel> rtn = new ReturnModel<UserDetailsModel> (Logger);

            try {
                #region Variables

                bool cnt = true;
                User getData = null;
                UserDTO user = null;
                UserDetailsModel userDetailsModel = null;
                ICollection<UserInGroup> userInGroupData = null;
                List<UserInGroupDTO> convertUserInGroupDataDTO = null;
                IEnumerable<IGrouping<int, UserInGroupDTO>> groupingGroups = null;
                ICollection<GroupInRole> groupInRoles = null;
                List<GroupInRoleDTO> convertedGroupInRoles = null;
                ICollection<UserInRole> userInRoles = null;

                #endregion Variables

                #region Action Body

                getData = Repository.Find (i => i.Id == id);
                if (getData == null) {
                    cnt = false;
                }

                if (cnt) {
                    user = Mapper.Map<UserDTO> (getData);
                    user.Password = string.Empty;

                    userDetailsModel = new UserDetailsModel {
                        User = user
                    };

                    userInGroupData = _userInGroupRepository.FindAll (i => i.UserId == user.Id);
                    if (userInGroupData.Any ()) {
                        convertUserInGroupDataDTO = Mapper.Map<List<UserInGroupDTO>> (userInGroupData);
                        userDetailsModel.InGroups.AddRange (convertUserInGroupDataDTO);

                        groupingGroups = convertUserInGroupDataDTO.GroupBy (i => i.GroupId).ToList ();

                        foreach (var groupId in groupingGroups) {
                            groupInRoles = _groupInRoleRepository.FindAll (i => i.GroupId == groupId.Key);
                            convertedGroupInRoles = Mapper.Map<List<GroupInRoleDTO>> (groupInRoles.ToList ());
                            if (convertedGroupInRoles.Any ()) {
                                foreach (var role in convertedGroupInRoles) {
                                    if (!userDetailsModel.InRoles.Any (i => i.RoleId == role.RoleId))
                                        userDetailsModel.InRoles.Add (new UserInRoleModel {
                                            RoleId = role.RoleId,
                                                GroupId = groupId.Key
                                        });
                                }
                            }
                        }
                    }

                    userInRoles = _userInRoleRepository.FindAll (i => i.UserId == user.Id);
                    if (userInRoles.Any ()) {
                        foreach (var inRole in userInRoles) {
                            if (!userDetailsModel.InRoles.Any (i => i.RoleId == inRole.RoleId && i.GroupId == 0))
                                userDetailsModel.InRoles.Add (new UserInRoleModel {
                                    RoleId = inRole.RoleId,
                                        GroupId = 0
                                });
                        }
                    }

                    rtn.Result = userDetailsModel;
                }

                #endregion Action Body

                #region Clear Memory

                getData = null;
                user = null;
                userDetailsModel = null;
                userInGroupData = null;
                convertUserInGroupDataDTO = null;
                groupingGroups = null;
                groupInRoles = null;
                convertedGroupInRoles = null;
                userInRoles = null;

                #endregion Clear Memory
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            return rtn;
        }

        #endregion Public Actions
    }
}