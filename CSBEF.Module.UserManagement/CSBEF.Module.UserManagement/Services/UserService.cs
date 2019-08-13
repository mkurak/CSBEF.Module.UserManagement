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

namespace CSBEF.Module.UserManagement.Services
{
    public class UserService : ServiceBase<User, UserDTO>, IUserService
    {
        #region Dependencies

        ITokenRepository _tokenRepository;
        IUserMessageRepository _userMessageRepository;

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
           IUserMessageRepository userMessageRepository
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

                /*
                 İŞ KURALLARI
                 -----------------------------------------------------------------
                 - Model validate olmalı
                 - UserId geçerli olmalı
                 - CurrentPass geçerli olmalı
                 - NewPass yeni şifre olarak kaydedilmeli
                 - Aktif olan tüm token'lar iptal edilmeli
                 */

                // (İK) - Model validate olmalı
                if (cnt)
                {
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePassword_ModelValidationFail);
                        cnt = false;
                    }
                }

                // (İK) - UserId geçerli olmalı
                if (cnt)
                {
                    getUser = await Repository.FindAsync(i => i.Id == args.Param.UserId);
                    if(getUser == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePassword_UserNotFound);
                        cnt = false;
                    }
                }

                // (İK) - CurrentPass geçerli olmalı
                if (cnt)
                {
                    if (getUser.Password.ToUpper() != args.Param.CurrentPass.ToUpper())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePassword_WrongCurrentPassword);
                        cnt = false;
                    }
                }

                // (İK) - NewPass yeni şifre olarak kaydedilmeli
                if (cnt)
                {
                    getUser.Password = args.Param.NewPass.ToUpper();
                    Repository.Update(getUser);
                    await Repository.SaveAsync();
                }

                // (İK) - Aktif olan tüm token'lar iptal edilmeli
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

                /*
                 İŞ KURALLARI
                 -----------------------------------------------------------------
                 - Model validate olmalı
                 - UserId geçerli olmalı
                 - Dosya, dosya adı değiştirilerek sunucuya kaydedilmeli
                 - Dosya, yeni adıyla ilgili kullanıcı kaydına işlenmeli
                 */

                // (İK) - Model validate olmalı
                if (cnt)
                {
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePicture_ModelValidationFail);
                        cnt = false;
                    }
                }

                // (İK) - UserId geçerli olmalı
                if (cnt)
                {
                    getUser = await Repository.FindAsync(i => i.Id == args.Param.Id);
                    if (getUser == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePicture_UserNotFound);
                        cnt = false;
                    }
                }

                // (İK) - Dosya, dosya adı değiştirilerek sunucuya kaydedilmeli
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

                // (İK) - Dosya, yeni adıyla ilgili kullanıcı kaydına işlenmeli
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

                /*
                 İŞ KURALLARI
                 -----------------------------------------------------------------
                 - Model validate olmalı
                 - UserId geçerli olmalı
                 - Dosya, dosya adı değiştirilerek sunucuya kaydedilmeli
                 - Dosya, yeni adıyla ilgili kullanıcı kaydına işlenmeli
                 */

                // (İK) - Model validate olmalı
                if (cnt)
                {
                    modelValidation = args.Param.ModelValidation();

                    if (modelValidation.Any())
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePicture_ModelValidationFail);
                        cnt = false;
                    }
                }

                // (İK) - UserId geçerli olmalı
                if (cnt)
                {
                    getUser = await Repository.FindAsync(i => i.Id == args.Param.Id);
                    if (getUser == null)
                    {
                        rtn = rtn.SendError(UserErrorsEnum.ChangePicture_UserNotFound);
                        cnt = false;
                    }
                }

                // (İK) - Dosya, dosya adı değiştirilerek sunucuya kaydedilmeli
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

                // (İK) - Dosya, yeni adıyla ilgili kullanıcı kaydına işlenmeli
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
                        var messages = await _userMessageRepository.FindAllAsync(i => (i.FromUserId == user.Id && i.ToUserId == getUser.Id) || (i.ToUserId == user.Id && i.FromUserId == getUser.Id));
                        if (messages.Any())
                        {
                            user.LastMessage = messages.Where(i => i.FromUserId == user.Id).OrderByDescending(i => i.AddingDate).First().AddingDate;

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

        #endregion Public Actions
    }
}