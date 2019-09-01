using AutoMapper;
using CSBEF.Module.UserManagement.Enums.Errors;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Core.Models.HelperModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSBEF.Core.Models.HubModels;

namespace CSBEF.Module.UserManagement.Services
{
    public class UserService : ServiceBase<User, UserDTO>, IUserService
    {
        #region Dependencies

        ITokenRepository _tokenRepository;
        IUserMessageRepository _userMessageRepository;
        IUserInGroupRepository _userInGroupRepository;
        IGroupInRoleRepository _groupInRoleRepository;
        IUserInRoleRepository _userInRoleRepository;
        IHubSyncDataService _hubSyncDataService;

        #endregion

        #region ctor

        public UserService(
           IHostingEnvironment hostingEnvironment,
           IConfiguration configuration,
           ILogger<ILog> logger,
           IMapper mapper,
           IUserRepository repository,
           IEventService eventService,

           // Other Repository Dependencies
           ITokenRepository tokenRepository,
           IUserMessageRepository userMessageRepository,
           IUserInGroupRepository userInGroupRepository,
           IGroupInRoleRepository groupInRoleRepository,
           IUserInRoleRepository userInRoleRepository,
           IHubSyncDataService hubSyncDataService
        ) : base(
           hostingEnvironment,
           configuration,
           logger,
           mapper,
           repository,
           eventService,
           "UserManagement",
           "UserService"
        )
        {
            _tokenRepository = tokenRepository;
            _userMessageRepository = userMessageRepository;
            _userInGroupRepository = userInGroupRepository;
            _groupInRoleRepository = groupInRoleRepository;
            _userInRoleRepository = userInRoleRepository;
            _hubSyncDataService = hubSyncDataService;
        }

        #endregion ctor

        #region Public Actions

        public async Task<IReturnModel<bool>> ChangePassword(ServiceParamsWithIdentifier<ChangePasswordModel> args)
        {
            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            try
            {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangePasswordModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                User getUser = null;
                string passwordHashSecureKey = _configuration["HashSecureKeys:Password"];

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangePassword.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangePasswordModel>>(args);
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
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePassword_ModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getUser = await Repository.FindAsync(i => i.Id == args.Param.UserId);
                    if(getUser == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePassword_UserNotFound);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    if (getUser.Password.ToUpper() != args.Param.CurrentPass.ToUpper())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePassword_WrongCurrentPassword);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getUser.Password = args.Param.NewPass.ToUpper();
                    Repository.Update(getUser);
                    await Repository.SaveAsync();
                }

                if (cnt)
                {
                    await _tokenRepository.KillUserTokens(args.Param.UserId, args.UserId);
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangePasswordModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ChangePassword"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangePassword.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangePasswordModel>>>(afterEventParameterModel);
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
                modelValidation = null;
                getUser = null;
                passwordHashSecureKey = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<string>> ChangePicture(ServiceParamsWithIdentifier<ChangePictureModel> args)
        {
            IReturnModel<string> rtn = new ReturnModel<string>(_logger);

            try
            {
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

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangePicture.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangePictureModel>>(args);
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
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePicture_ModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getUser = await Repository.FindAsync(i => i.Id == args.Param.Id);
                    if (getUser == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePicture_UserNotFound);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    uploaderRootPath = Path.Combine(GlobalConfiguration.sWwwRootPath, _configuration["AppSettings:FileUploader:UserManagement:ProfilePicRootPath"]);
                    newFileUpload = new FileUploader(_configuration, _logger, null, 0, 0, uploaderRootPath);
                    savedFileName = await newFileUpload.Upload(args.Param.Picture);
                    if (savedFileName.Error.Status)
                    {
                        rtn.Error = savedFileName.Error;
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    if (!string.IsNullOrWhiteSpace(getUser.ProfilePic))
                    {
                        targetRemoveFile = Path.Combine(uploaderRootPath, getUser.ProfilePic);
                        File.Delete(targetRemoveFile);
                    }

                    getUser.ProfilePic = savedFileName.Result;
                    Repository.Update(getUser);
                    await Repository.SaveAsync();

                    rtn.Result = getUser.ProfilePic;
                }

                if (cnt)
                {
                    await _hubSyncDataService.OnSync(new HubSyncDataModel<UserDetailsModel> {
                        Key = "UserManagement_User",
                        ProcessType = "update",
                        Id = args.Param.Id,
                        UserId = args.UserId,
                        Name = getUser.Name + " " + getUser.Surname,
                        Data = (await GetUserDetails(args.Param.Id)).Result
                    });
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ChangePicture"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangePicture.After")
                        .EventHandler<string, IAfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>>>(afterEventParameterModel);
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
                modelValidation = null;
                getUser = null;
                uploaderRootPath = null;
                newFileUpload = null;
                savedFileName = null;
                targetRemoveFile = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<string>> ChangeBgPicture(ServiceParamsWithIdentifier<ChangePictureModel> args)
        {
            IReturnModel<string> rtn = new ReturnModel<string>(_logger);

            try
            {
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

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangeBgPicture.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangePictureModel>>(args);
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
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePicture_ModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getUser = await Repository.FindAsync(i => i.Id == args.Param.Id);
                    if (getUser == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePicture_UserNotFound);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    uploaderRootPath = Path.Combine(GlobalConfiguration.sWwwRootPath, _configuration["AppSettings:FileUploader:UserManagement:ProfileBgPicRootPath"]);
                    newFileUpload = new FileUploader(_configuration, _logger, null, 0, 0, uploaderRootPath);
                    savedFileName = await newFileUpload.Upload(args.Param.Picture);
                    if (savedFileName.Error.Status)
                    {
                        rtn.Error = savedFileName.Error;
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    if (!string.IsNullOrWhiteSpace(getUser.ProfileBgPic))
                    {
                        targetRemoveFile = Path.Combine(uploaderRootPath, getUser.ProfilePic);
                        File.Delete(targetRemoveFile);
                    }

                    getUser.ProfileBgPic = savedFileName.Result;
                    Repository.Update(getUser);
                    await Repository.SaveAsync();

                    rtn.Result = getUser.ProfilePic;
                }

                if (cnt)
                {
                    await _hubSyncDataService.OnSync(new HubSyncDataModel<UserDetailsModel>
                    {
                        Key = "UserManagement_User",
                        ProcessType = "update",
                        Id = args.Param.Id,
                        UserId = args.UserId,
                        Name = getUser.Name + " " + getUser.Surname,
                        Data = (await GetUserDetails(args.Param.Id)).Result
                    });
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ChangeBgPicture"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangeBgPicture.After")
                        .EventHandler<string, IAfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<ChangePictureModel>>>(afterEventParameterModel);
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
                modelValidation = null;
                getUser = null;
                uploaderRootPath = null;
                newFileUpload = null;
                savedFileName = null;
                targetRemoveFile = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<IList<UserForCurrentUser>>> UserListForCurrentUser(ServiceParamsWithIdentifier<int> args)
        {
            IReturnModel<IList<UserForCurrentUser>> rtn = new ReturnModel<IList<UserForCurrentUser>>(_logger);

            try
            {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<IList<UserForCurrentUser>>, ServiceParamsWithIdentifier<int>> afterEventParameterModel = null;
                IReturnModel<IList<UserForCurrentUser>> afterEventHandler = null;
                User getUser = null;
                ICollection<User> users = null;
                IList<UserForCurrentUser> convertList = null;
                ICollection<UserMessage> messages = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.UserListForCurrentUser.Before").EventHandler<bool, ServiceParamsWithIdentifier<int>>(args);
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
                    getUser = await Repository.FindAsync(i => i.Id == args.Param);
                    if (getUser == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.UserListForCurrentUser_UserNotFound);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    users = await Repository.FindAllAsync(i => i.Status == true);
                    cnt = users.Any();
                }

                if (cnt)
                {
                    convertList = _mapper.Map<List<UserForCurrentUser>>(users);

                    foreach (var user in convertList)
                    {
                        messages = await _userMessageRepository.FindAllAsync(i => (i.FromUserId == user.Id && i.ToUserId == getUser.Id) || (i.ToUserId == user.Id && i.FromUserId == getUser.Id));
                        if (messages.Any())
                        {
                            var lastMessage = messages.Where(i => i.FromUserId == user.Id).OrderByDescending(i => i.AddingDate).FirstOrDefault();
                            if(lastMessage != null)
                                user.LastMessage = lastMessage.AddingDate;

                            foreach (var message in messages)
                            {
                                var viewStatus = true;

                                if (message.FromUserId != getUser.Id)
                                {
                                    viewStatus = message.ViewStatus;
                                }

                                user.Messages.Add(new MessageModelForCurrentUser
                                {
                                    Id = message.Id,
                                    OwnerMe = message.FromUserId == getUser.Id,
                                    Message = message.Message,
                                    ViewStatus = viewStatus,
                                    SendDate = message.AddingDate,
                                    ViewDate = message.ViewDate
                                });
                            }
                        }
                    }

                    rtn.Result = convertList;
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<IList<UserForCurrentUser>>, ServiceParamsWithIdentifier<int>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "UserListForCurrentUser"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.UserListForCurrentUser.After")
                        .EventHandler<IList<UserForCurrentUser>, IAfterEventParameterModel<IReturnModel<IList<UserForCurrentUser>>, ServiceParamsWithIdentifier<int>>>(afterEventParameterModel);
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
                getUser = null;
                users = null;
                convertList = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<bool>> ChangeProfileInformations(ServiceParamsWithIdentifier<ChangeProfileInformationsModel> args)
        {
            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            try
            {
                #region Variables
                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeProfileInformationsModel>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                List<ValidationResult> modelValidation = null;
                User getData = null;
                #endregion

                #region Before Event Handler
                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangeProfileInformations.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangeProfileInformationsModel>>(args);
                if (beforeEventHandler != null)
                {
                    if (beforeEventHandler.Error.Status)
                    {
                        rtn.Error = beforeEventHandler.Error;
                        cnt = false;
                    }
                }
                #endregion

                #region Action Body

                /*
                 İŞ KURALLARI
                 -----------------------------------------------------------------
                 - Model validate olmalı
                 - Id geçerli olmalı
                 - İlgili tablodaki Name, Surname ve ProfileStatusMessage alanları gelen verilerle güncellenmeli
                 */

                // (İK) - Model validate olmalı
                if (cnt)
                {
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangeProfileInformations_ModelValidationFail);
                        cnt = false;
                    }
                }

                // (İK) - Id geçerli olmalı
                if (cnt)
                {
                    getData = await Repository.FindAsync(i => i.Id == args.Param.Id);
                    if (getData == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangeProfileInformations_DataFound);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getData.Name = args.Param.Name;
                    getData.Surname = args.Param.Surname;
                    getData.ProfileStatusMessage = args.Param.ProfileStatusMessage;
                    getData.UpdatingDate = DateTime.Now;
                    getData.UpdatingUserId = args.UserId;
                    Repository.Update(getData);
                    await Repository.SaveAsync();
                }

                rtn.Result = cnt;

                #endregion

                #region After Event Handler
                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeProfileInformationsModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "Add"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangeProfileInformations.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeProfileInformationsModel>>>(afterEventParameterModel);
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
                #endregion

                #region Clear Memory
                args = null;
                beforeEventHandler = null;
                afterEventParameterModel = null;
                afterEventHandler = null;
                modelValidation = null;
                getData = null;
                #endregion
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<IList<UserDetailsModel>>> ListWithDetails(ServiceParamsWithIdentifier<ActionFilterModel> args)
        {
            IReturnModel<IList<UserDetailsModel>> rtn = new ReturnModel<IList<UserDetailsModel>>(_logger);

            try
            {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<IList<UserDetailsModel>>, ServiceParamsWithIdentifier<ActionFilterModel>> afterEventParameterModel = null;
                IReturnModel<IList<UserDetailsModel>> afterEventHandler = null;
                IReturnModel<IList<UserDTO>> baseList = null;
                IReturnModel<UserDetailsModel> getUserDetails = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ListWithDetails.Before").EventHandler<bool, ServiceParamsWithIdentifier<ActionFilterModel>>(args);
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
                    rtn.Result = new List<UserDetailsModel>();

                    baseList = await base.ListAsync(args.Param);
                    if (!baseList.Result.Any())
                        cnt = false;
                }

                if (cnt)
                {
                    foreach(var user in baseList.Result)
                    {
                        getUserDetails = await GetUserDetails(user.Id);
                        if (getUserDetails.Error.Status)
                        {
                            rtn.Error = getUserDetails.Error;
                            cnt = false;
                        }
                        else
                        {
                            if(getUserDetails.Result != null)
                                rtn.Result.Add(getUserDetails.Result);
                        }
                    }
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<IList<UserDetailsModel>>, ServiceParamsWithIdentifier<ActionFilterModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ListWithDetails"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ListWithDetails.After")
                        .EventHandler<IList<UserDetailsModel>, IAfterEventParameterModel<IReturnModel<IList<UserDetailsModel>>, ServiceParamsWithIdentifier<ActionFilterModel>>>(afterEventParameterModel);
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
                baseList = null;
                getUserDetails = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<UserDTO>> Save(ServiceParamsWithIdentifier<SaveUserModel> args)
        {
            IReturnModel<UserDTO> rtn = new ReturnModel<UserDTO>(_logger);

            try
            {
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

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.Save.Before").EventHandler<bool, ServiceParamsWithIdentifier<SaveUserModel>>(args);
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
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.Save_ModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    if(args.Param.Id > 0)
                        checkEmail = await Repository.FindAsync(i => i.Email == args.Param.Email && i.Id != args.Param.Id);
                    else
                        checkEmail = await Repository.FindAsync(i => i.Email == args.Param.Email);

                    if(checkEmail != null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.Save_EmailExists);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    if(args.Param.Id > 0)
                        checkUserName = await Repository.FindAsync(i => i.UserName == args.Param.UserName && i.Id != args.Param.Id);
                    else
                        checkUserName = await Repository.FindAsync(i => i.UserName == args.Param.UserName);

                    if(checkUserName != null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.Save_UserNameExists);
                        cnt = false;
                    }
                }

                if (cnt && args.Param.Id > 0)
                {
                    getData = await Repository.FindAsync(i => i.Id == args.Param.Id);
                    if (getData == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.Save_UserNotFound);
                        cnt = false;
                    }
                }

                if (cnt && args.Param.Id > 0)
                {
                    getData.Name = args.Param.Name;
                    getData.Surname = args.Param.Surname;
                    getData.UserName = args.Param.UserName;
                    getData.Email = args.Param.Email;
                    getData.UpdatingDate = DateTime.Now;
                    getData.UpdatingUserId = args.UserId;
                }

                if(cnt && args.Param.Id > 0 && !string.IsNullOrWhiteSpace(args.Param.CurrentPassword))
                {
                    if(getData.Password.ToUpper() != args.Param.CurrentPassword.ToUpper())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.Save_CurrentPasswordWrong);
                        cnt = false;
                    }
                    else
                    {
                        getData.Password = args.Param.NewPassword;
                    }
                }

                if(cnt && args.Param.Id == 0 && string.IsNullOrWhiteSpace(args.Param.NewPassword))
                {
                    rtn = rtn.SendError(UserErrorsEnum.Save_PasswordRequired);
                    cnt = false;
                }

                if(cnt && args.Param.Id == 0)
                {
                    saveData = new User
                    {
                        Name = args.Param.Name,
                        Surname = args.Param.Surname,
                        UserName = args.Param.UserName,
                        Email = args.Param.Email,
                        Password = args.Param.NewPassword,
                        ProfilePic = "default.jpg",
                        ProfileBgPic = "default.jpg",
                        ProfileStatusMessage = "Hello Worl!",
                        AddingDate = DateTime.Now,
                        UpdatingDate = DateTime.Now,
                        AddingUserId = args.UserId,
                        UpdatingUserId = args.UserId
                    };

                    savedData = Repository.Add(saveData);
                    await Repository.SaveAsync();
                    rtn.Result = _mapper.Map<UserDTO>(savedData);
                }

                if(cnt && args.Param.Id > 0)
                {
                    savedData = Repository.Update(getData);
                    await Repository.SaveAsync();
                    rtn.Result = _mapper.Map<UserDTO>(savedData);

                    getTokens = await _tokenRepository.FindAllAsync(i => i.UserId == args.Param.Id);
                    if (getTokens.Any())
                    {
                        foreach(var token in getTokens)
                        {
                            token.Status = false;
                            token.UpdatingDate = DateTime.Now;
                            token.UpdatingUserId = args.UserId;
                            _tokenRepository.Update(token);
                        }

                        await Repository.SaveAsync();
                    }
                }

                if (cnt)
                {
                    await _hubSyncDataService.OnSync(new HubSyncDataModel<UserDetailsModel>
                    {
                        Key = "UserManagement_User",
                        ProcessType = args.Param.Id > 0 ? "update" : "add",
                        Id = args.Param.Id.ToInt(0),
                        UserId = args.UserId,
                        Name = savedData.Name + " " + savedData.Surname,
                        Data = (await GetUserDetails(savedData.Id)).Result
                    });
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<UserDTO>, ServiceParamsWithIdentifier<SaveUserModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "Save"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.Save.After")
                        .EventHandler<UserDTO, IAfterEventParameterModel<IReturnModel<UserDTO>, ServiceParamsWithIdentifier<SaveUserModel>>>(afterEventParameterModel);
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
                modelValidation = null;
                checkEmail = null;
                checkUserName = null;
                getData = null;
                saveData = null;
                savedData = null;
                getTokens = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<bool>> ChangeStatus(ServiceParamsWithIdentifier<ChangeStatusModel> args)
        {
            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            try
            {
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

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangeStatus.Before").EventHandler<bool, ServiceParamsWithIdentifier<ChangeStatusModel>>(args);
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
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangeStatus_ModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getData = await Repository.FindAsync(i => i.Id == args.Param.Id);
                    if (getData == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangeStatus_DataNotFound);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    getData.Status = args.Param.Status;
                    getData.UpdatingDate = DateTime.Now;
                    getData.UpdatingUserId = args.UserId;
                    getData = Repository.Update(getData);
                    await Repository.SaveAsync();
                }

                if (cnt)
                {
                    tokens = await _tokenRepository.FindAllAsync(i => i.UserId == args.Param.Id);
                    if (tokens.Any())
                    {
                        foreach(var token in tokens)
                        {
                            token.Status = false;
                            token.UpdatingDate = DateTime.Now;
                            token.UpdatingUserId = args.UserId;
                            _tokenRepository.Update(token);
                        }
                        await _tokenRepository.SaveAsync();
                    }
                }

                if (cnt)
                {
                    await _hubSyncDataService.OnSync(new HubSyncDataModel<int>
                    {
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

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeStatusModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "ChangeStatus"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.ChangeStatus.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<ChangeStatusModel>>>(afterEventParameterModel);
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
                modelValidation = null;
                getData = null;
                tokens = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<bool>> SaveUserInGroups(ServiceParamsWithIdentifier<SaveUserInGroupsModel> args)
        {
            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            try
            {
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

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.SaveUserInGroups.Before").EventHandler<bool, ServiceParamsWithIdentifier<SaveUserInGroupsModel>>(args);
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
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.SaveUserInGroups_ModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    items = await _userInGroupRepository.FindAllAsync(i => i.UserId == args.Param.UserId);
                    if (items.Any())
                    {
                        foreach(var item in items)
                        {
                            _userInGroupRepository.Delete(item);
                        }

                        await _userInGroupRepository.SaveAsync();
                    }

                    groupsArray = args.Param.Groups.Split(",");

                    if (groupsArray.Any())
                    {
                        foreach(var group in groupsArray)
                        {
                            var convertIntGroup = group.ToInt(0);
                            if(convertIntGroup > 0)
                            {
                                _userInGroupRepository.Add(new UserInGroup
                                {
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

                        await _userInGroupRepository.SaveAsync();
                    }

                    tokens = await _tokenRepository.FindAllAsync(i => i.UserId == args.Param.UserId);
                    if (tokens.Any())
                    {
                        foreach(var token in tokens)
                        {
                            token.Status = false;
                            token.UpdatingDate = DateTime.Now;
                            token.UpdatingUserId = args.UserId;
                            _tokenRepository.Update(token);
                        }

                        await _userInGroupRepository.SaveAsync();
                    }
                }

                if (cnt)
                {
                    userDetails = await GetUserDetails(args.Param.UserId);

                    await _hubSyncDataService.OnSync(new HubSyncDataModel<UserDetailsModel>
                    {
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

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInGroupsModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "SaveUserInGroups"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.SaveUserInGroups.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInGroupsModel>>>(afterEventParameterModel);
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
                modelValidation = null;
                items = null;
                tokens = null;
                userDetails = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<bool>> SaveUserInRoles(ServiceParamsWithIdentifier<SaveUserInRolesModel> args)
        {
            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            try
            {
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

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.SaveUserInRoles.Before").EventHandler<bool, ServiceParamsWithIdentifier<SaveUserInRolesModel>>(args);
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
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.SaveUserInRoles_ModelValidationFail);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    items = await _userInRoleRepository.FindAllAsync(i => i.UserId == args.Param.UserId);
                    if (items.Any())
                    {
                        foreach (var item in items)
                        {
                            _userInRoleRepository.Delete(item);
                        }

                        await _userInRoleRepository.SaveAsync();
                    }

                    rolesArray = args.Param.Roles.Split(",");

                    if (rolesArray.Any())
                    {
                        foreach (var role in rolesArray)
                        {
                            var convertIntrole = role.ToInt(0);
                            if (convertIntrole > 0)
                            {
                                _userInRoleRepository.Add(new UserInRole
                                {
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

                        await _userInRoleRepository.SaveAsync();
                    }

                    tokens = await _tokenRepository.FindAllAsync(i => i.UserId == args.Param.UserId);
                    if (tokens.Any())
                    {
                        foreach (var token in tokens)
                        {
                            token.Status = false;
                            token.UpdatingDate = DateTime.Now;
                            token.UpdatingUserId = args.UserId;
                            _tokenRepository.Update(token);
                        }

                        await _userInGroupRepository.SaveAsync();
                    }
                }

                if (cnt)
                {
                    userDetails = await GetUserDetails(args.Param.UserId);

                    await _hubSyncDataService.OnSync(new HubSyncDataModel<UserDetailsModel>
                    {
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

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInRolesModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "SaveUserInRoles"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.SaveUserInRoles.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<SaveUserInRolesModel>>>(afterEventParameterModel);
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
                modelValidation = null;
                items = null;
                tokens = null;
                userDetails = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        private async Task<IReturnModel<UserDetailsModel>> GetUserDetails(int id)
        {
            IReturnModel<UserDetailsModel> rtn = new ReturnModel<UserDetailsModel>(_logger);

            try
            {
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

                getData = await Repository.FindAsync(i => i.Id == id);
                if(getData == null)
                {
                    cnt = false;
                }

                if (cnt)
                {
                    user = _mapper.Map<UserDTO>(getData);
                    user.Password = "";

                    userDetailsModel = new UserDetailsModel
                    {
                        User = user,
                        InGroups = new List<UserInGroupDTO>(),
                        InRoles = new List<UserInRoleModel>()
                    };

                    userInGroupData = await _userInGroupRepository.FindAllAsync(i => i.UserId == user.Id);
                    if (userInGroupData.Any())
                    {
                        convertUserInGroupDataDTO = _mapper.Map<List<UserInGroupDTO>>(userInGroupData);
                        userDetailsModel.InGroups.AddRange(convertUserInGroupDataDTO);

                        groupingGroups = convertUserInGroupDataDTO.GroupBy(i => i.GroupId).ToList();

                        foreach (var groupId in groupingGroups)
                        {
                            groupInRoles = await _groupInRoleRepository.FindAllAsync(i => i.GroupId == groupId.Key);
                            convertedGroupInRoles = _mapper.Map<List<GroupInRoleDTO>>(groupInRoles.ToList());
                            if (convertedGroupInRoles.Any())
                            {
                                foreach (var role in convertedGroupInRoles)
                                {
                                    if (!userDetailsModel.InRoles.Any(i => i.RoleId == role.RoleId))
                                        userDetailsModel.InRoles.Add(new UserInRoleModel
                                        {
                                            RoleId = role.RoleId,
                                            GroupId = groupId.Key
                                        });
                                }
                            }
                        }
                    }

                    userInRoles = await _userInRoleRepository.FindAllAsync(i => i.UserId == user.Id);
                    if (userInRoles.Any())
                    {
                        foreach (var inRole in userInRoles)
                        {
                            if (!userDetailsModel.InRoles.Any(i => i.RoleId == inRole.RoleId && i.GroupId == 0))
                                userDetailsModel.InRoles.Add(new UserInRoleModel
                                {
                                    RoleId = inRole.RoleId,
                                    GroupId = 0
                                });
                        }
                    }

                    rtn.Result = userDetailsModel;
                }

                #endregion

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

                #endregion
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        #endregion Public Actions
    }
}