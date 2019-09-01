using AutoMapper;
using CSBEF.Module.UserManagement.Enums.Errors;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Services
{
    public class TokenService : ServiceBase<Token, TokenDTO>, ITokenService
    {
        #region Other Repositories

        private IUserRepository _userRepository;
        private IUserInGroupRepository _userInGroupRepository;
        private IGroupInRoleRepository _groupInRoleRepository;
        private IUserInRoleRepository _userInRoleRepository;
        private IRoleRepository _roleRepository;

        #endregion Other Repositories

        #region ctor

        public TokenService(
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            ILogger<ILog> logger,
            IMapper mapper,
            ITokenRepository repository,
            IEventService eventService,

            // Other Repository Dependencies
            IUserRepository userRepository,
            IUserInGroupRepository userInGroupRepository,
            IGroupInRoleRepository groupInRoleRepository,
            IUserInRoleRepository userInRoleRepository,
            IRoleRepository roleRepository
        ) : base(
            hostingEnvironment,
            configuration,
            logger,
            mapper,
            repository,
            eventService,
            "UserManagement",
            "TokenService"
        )
        {
            _userRepository = userRepository;
            _userInGroupRepository = userInGroupRepository;
            _groupInRoleRepository = groupInRoleRepository;
            _userInRoleRepository = userInRoleRepository;
            _roleRepository = roleRepository;
        }

        #endregion ctor

        #region Public Actions

        public async Task<IReturnModel<string>> CreateToken(ServiceParamsWithIdentifier<CreateTokenModel> args)
        {
            IReturnModel<string> rtn = new ReturnModel<string>(_logger);
            rtn.Result = "";

            try
            {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<CreateTokenModel>> afterEventParameterModel = null;
                IReturnModel<string> afterEventHandler = null;
                User getUser = null;
                Token getToken = null;
                IReturnModel<IList<RoleDTO>> getUserRoles = null;
                JwtSecurityTokenHandler tokenHandler = null;
                byte[] key = null;
                SecurityTokenDescriptor tokenDescriptor = null;
                SecurityToken token = null;
                string tokenString = null;
                Token saveTokenModel = null;
                string passwordHashSecureKey = _configuration["HashSecureKeys:Password"];

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.CreateToken.Before").EventHandler<bool, ServiceParamsWithIdentifier<CreateTokenModel>>(args);
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
                    if (args == null)
                    {
                        rtn = rtn.SendError(TokenErrorsEnum.CreateToken_ArgsNull);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    if (args.Param == null)
                    {
                        rtn = rtn.SendError(TokenErrorsEnum.CreateToken_ArgsParamNull);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    if (!string.IsNullOrWhiteSpace(args.Param.UserName))
                    {
                        if (!args.Param.UserName.UserNameIsValid())
                        {
                            rtn = rtn.SendError(TokenErrorsEnum.CreateToken_UserNameInValid);
                            cnt = false;
                        }
                    }
                    else
                    {
                        if (!args.Param.Email.IsValidEmail())
                        {
                            rtn = rtn.SendError(TokenErrorsEnum.CreateToken_EmailInValid);
                            cnt = false;
                        }
                    }
                }

                if (cnt)
                {
                    if (string.IsNullOrWhiteSpace(args.Param.Device))
                    {
                        rtn = rtn.SendError(TokenErrorsEnum.CreateToken_DeviceEmpty);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    if (string.IsNullOrWhiteSpace(args.Param.DeviceKey))
                    {
                        rtn = rtn.SendError(TokenErrorsEnum.CreateToken_DeviceKeyEmpty);
                        cnt = false;
                    }
                }

                if (cnt)
                {
                    if (!string.IsNullOrWhiteSpace(args.Param.UserName))
                    {
                        getUser = await _userRepository.GetAll().FirstOrDefaultAsync(i => i.UserName == args.Param.UserName);
                        if (getUser == null)
                        {
                            rtn = rtn.SendError(TokenErrorsEnum.CreateToken_UserNotFound);
                            cnt = false;
                        }
                    }
                    else
                    {
                        getUser = await _userRepository.GetAll().FirstOrDefaultAsync(i => i.Email == args.Param.Email);
                        if (getUser == null)
                        {
                            rtn = rtn.SendError(TokenErrorsEnum.CreateToken_UserNotFound);
                            cnt = false;
                        }
                    }
                }

                if (cnt)
                {
                    if (getUser.Password.ToUpper() != args.Param.Password.ToUpper())
                    {
                        cnt = false;
                        rtn = rtn.SendError(TokenErrorsEnum.CreateToken_WrongPass);
                    }
                }

                if (cnt)
                {
                    getToken = await Repository.GetAll().FirstOrDefaultAsync(i => i.UserId == getUser.Id && i.DeviceKey == args.Param.DeviceKey && i.Status == true);
                    if (getToken != null)
                    {
                        rtn.Result = getToken.TokenCode;
                    }
                }

                if (cnt && string.IsNullOrWhiteSpace(rtn.Result))
                {
                    getUserRoles = await GetUserRoleList(getUser.Id);
                    if (getUserRoles.Error.Status)
                    {
                        rtn.Error = getUserRoles.Error;
                        cnt = false;
                    }
                }

                if(cnt && string.IsNullOrWhiteSpace(rtn.Result))
                {
                    saveTokenModel = new Token
                    {
                        Status = true,
                        AddingDate = DateTime.Now,
                        UpdatingDate = DateTime.Now,
                        AddingUserId = getUser.Id,
                        UpdatingUserId = getUser.Id,
                        UserId = getUser.Id,
                        TokenCode = "[tokenInComing]",
                        ExpiredDate = DateTime.UtcNow.AddDays(_configuration["AppSettings:JWTSettings:ExpireDays"].ToInt(365)),
                        Device = args.Param.Device,
                        DeviceKey = args.Param.DeviceKey
                    };
                    saveTokenModel = Repository.Add(saveTokenModel);
                    await Repository.SaveAsync();
                }

                if (cnt && string.IsNullOrWhiteSpace(rtn.Result))
                {
                    tokenHandler = new JwtSecurityTokenHandler();
                    key = Encoding.ASCII.GetBytes(_configuration["AppSettings:JWTSettings:SecretCode"]);
                    tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Audience = "CSBEF",
                        Issuer = "JWT.CSBEF",
                        Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, getUser.Id.ToString()),
                        new Claim(ClaimTypes.Email, getUser.Email),
                        new Claim(ClaimTypes.GivenName, args.Param.Device),
                        new Claim(ClaimTypes.SerialNumber, args.Param.DeviceKey),
                        new Claim("UserName", getUser.Name),
                        new Claim("UserSurname", getUser.Surname),
                        new Claim("ProfilePic", getUser.ProfilePic),
                        new Claim("ProfileBgPic", getUser.ProfileBgPic),
                        new Claim("ProfileStatusMessage", getUser.ProfileStatusMessage),
                        new Claim("TokenId", saveTokenModel.Id.ToString())
                    }),
                        Expires = DateTime.UtcNow.AddDays(_configuration["AppSettings:JWTSettings:ExpireDays"].ToInt(365)),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    if (getUserRoles.Result.Any())
                    {
                        foreach (var role in getUserRoles.Result)
                        {
                            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
                        }
                    }

                    token = tokenHandler.CreateToken(tokenDescriptor);
                    tokenString = tokenHandler.WriteToken(token);
                }

                if (cnt && string.IsNullOrWhiteSpace(rtn.Result) && !string.IsNullOrWhiteSpace(tokenString))
                {
                    saveTokenModel.TokenCode = tokenString;

                    Repository.Update(saveTokenModel);
                    await Repository.SaveAsync();

                    rtn.Result = tokenString;
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<CreateTokenModel>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "CreateToken"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.CreateToken.After")
                        .EventHandler<string, IAfterEventParameterModel<IReturnModel<string>, ServiceParamsWithIdentifier<CreateTokenModel>>>(afterEventParameterModel);
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
                getToken = null;
                getUserRoles = null;
                tokenHandler = null;
                key = null;
                tokenDescriptor = null;
                token = null;
                tokenString = null;
                saveTokenModel = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        public async Task<IReturnModel<bool>> CheckToken(ServiceParamsWithIdentifier<string> args)
        {
            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            try
            {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<string>> afterEventParameterModel = null;
                IReturnModel<bool> afterEventHandler = null;
                string stream = null;
                JwtSecurityTokenHandler handler = null;
                SecurityToken jsonToken = null;
                JwtSecurityToken tokenS = null;
                Claim userId = null;
                Claim deviceKey = null;
                List<Token> getTokens = null;
                Token getToken = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.CheckToken.Before").EventHandler<bool, ServiceParamsWithIdentifier<string>>(args);
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
                    stream = args.Param;
                    handler = new JwtSecurityTokenHandler();
                    jsonToken = handler.ReadToken(stream);
                    tokenS = handler.ReadToken(stream) as JwtSecurityToken;

                    userId = tokenS.Claims.FirstOrDefault(i => i.Type == "unique_name");
                    if (userId == null)
                        cnt = false;
                }

                if (cnt)
                {
                    deviceKey = tokenS.Claims.FirstOrDefault(i => i.Type == "certserialnumber");
                    if (deviceKey == null)
                        cnt = false;
                }

                if (cnt)
                {
                    getTokens = await Repository.GetAll().Where(i => i.UserId == userId.Value.ToInt(0) && i.DeviceKey == deviceKey.Value).ToListAsync();
                    cnt = getTokens.Any();
                }

                if (cnt)
                {
                    getToken = getTokens.FirstOrDefault(i => i.TokenCode == args.Param);
                    if (getToken == null)
                        cnt = false;
                    else if (!getToken.Status)
                        cnt = false;
                    else if (getToken.ExpiredDate < DateTime.Now)
                        cnt = false;
                }

                rtn.Result = cnt;

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<string>>
                    {
                        DataToBeSent = rtn,
                        ActionParameter = args,
                        ModuleName = ModuleName,
                        ServiceName = ServiceName,
                        ActionName = "CheckToken"
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.CreateToken.After")
                        .EventHandler<bool, IAfterEventParameterModel<IReturnModel<bool>, ServiceParamsWithIdentifier<string>>>(afterEventParameterModel);
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
                stream = null;
                handler = null;
                jsonToken = null;
                tokenS = null;
                userId = null;
                deviceKey = null;
                getTokens = null;
                getToken = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        #endregion Public Actions

        #region Private Helpers

        private async Task<IReturnModel<IList<RoleDTO>>> GetUserRoleList(int userId)
        {
            IReturnModel<IList<RoleDTO>> rtn = new ReturnModel<IList<RoleDTO>>(_logger);

            try
            {
                #region Variables

                bool cnt = true;
                IReturnModel<bool> beforeEventHandler = null;
                AfterEventParameterModel<IList<RoleDTO>, int> afterEventParameterModel = null;
                IReturnModel<IList<RoleDTO>> afterEventHandler = null;
                IList<UserInGroup> getGroups = null;
                IList<GroupInRole> getGroupInRoles = null;
                List<int> roles = new List<int>();
                List<int> roles2 = new List<int>();
                IList<UserInRole> getUserInRoles = null;
                Role getRole = null;

                #endregion Variables

                #region Before Event Handler

                beforeEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.GetUserRoleList.Before").EventHandler<bool, int>(userId);
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
                    rtn.Result = new List<RoleDTO>();
                    getGroups = await _userInGroupRepository.GetAll().Where(i => i.UserId == userId).ToListAsync();
                    if (getGroups.Any())
                    {
                        foreach (var item in getGroups)
                        {
                            getGroupInRoles = await _groupInRoleRepository.GetAll().Where(i => i.GroupId == item.GroupId).ToListAsync();
                            if (getGroupInRoles.Any())
                            {
                                roles.AddRange(getGroupInRoles.Select(i => i.RoleId));
                            }
                        }
                    }

                    getUserInRoles = await _userInRoleRepository.GetAll().Where(i => i.UserId == userId).ToListAsync();
                    if (getUserInRoles.Any())
                    {
                        roles.AddRange(getUserInRoles.Select(i => i.RoleId));
                    }

                    if (roles.Any())
                    {
                        roles2 = roles.Distinct().ToList();
                        foreach (var role in roles2)
                        {
                            if (!cnt)
                                continue;

                            getRole = await _roleRepository.FindAsync(i => i.Id == role);
                            rtn.Result.Add(_mapper.Map<RoleDTO>(getRole));
                        }
                    }
                }

                #endregion Action Body

                #region After Event Handler

                if (cnt)
                {
                    afterEventParameterModel = new AfterEventParameterModel<IList<RoleDTO>, int>
                    {
                        DataToBeSent = rtn.Result,
                        ActionParameter = userId
                    };
                    afterEventHandler = await _eventService.GetEvent(ModuleName, $"{ServiceName}.GetUserRoleList.After")
                        .EventHandler<IList<RoleDTO>, IAfterEventParameterModel<IList<RoleDTO>, int>>(afterEventParameterModel);
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

                beforeEventHandler = null;
                afterEventParameterModel = null;
                afterEventHandler = null;
                getGroups = null;
                getGroupInRoles = null;
                roles = null;
                roles2 = null;
                getUserInRoles = null;
                getRole = null;

                #endregion Clear Memory
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            return rtn;
        }

        #endregion Private Helpers
    }
}