using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Core.Models.HelperModels;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Models.Return;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSBEF.Module.UserManagement.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Dependencies

        private readonly IConfiguration _configuration;
        private readonly ILogger<ILog> _logger;
        private readonly IUserService _service;

        #endregion Dependencies

        #region Construction

        public UserController(IConfiguration configuration, ILogger<ILog> logger, IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _service = userService;
        }

        #endregion Construction

        #region Actions

        [Authorize]
        [Route("api/UserManagement/User/ChangePassword")]
        [HttpPost]
        public ActionResult<IReturnModel<bool>> ChangePassword([FromBody]ChangePasswordModel data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            #region Declares

            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            #endregion Declares

            #region Hash Control

            if (!data.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ChangePassword"]))
            {
                _logger.LogError("InvalidHash: " + data.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);

                var serviceFilterModel = new ServiceParamsWithIdentifier<ChangePasswordModel>(data, userId, tokenId);
                IReturnModel<bool> serviceAction = _service.ChangePassword(serviceFilterModel);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Authorize]
        [Route("api/UserManagement/User/ChangeProfilePicture")]
        [HttpPost]
        public ActionResult<IReturnModel<string>> ChangeProfilePicture([FromForm]ChangePictureModel data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            #region Declares

            IReturnModel<string> rtn = new ReturnModel<string>(_logger);

            #endregion Declares

            #region Hash Control

            if (!data.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ChangeProfilePicture"]))
            {
                _logger.LogError("InvalidHash: " + data.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region PictureType Control

            if (data.PictureType != "User.ProfilePicture")
            {
                rtn = rtn.SendError(GlobalErrors.WrongPictureType);
                return Ok(rtn);
            }

            #endregion PictureType Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);

                var serviceFilterModel = new ServiceParamsWithIdentifier<ChangePictureModel>(data, userId, tokenId);
                IReturnModel<string> serviceAction = _service.ChangePicture(serviceFilterModel);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Authorize]
        [Route("api/UserManagement/User/ChangeProfileBgPicture")]
        [HttpPost]
        public ActionResult<IReturnModel<string>> ChangeProfileBgPicture([FromForm]ChangePictureModel data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            #region Declares

            IReturnModel<string> rtn = new ReturnModel<string>(_logger);

            #endregion Declares

            #region Hash Control

            if (!data.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ChangeProfileBgPicture"]))
            {
                _logger.LogError("InvalidHash: " + data.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region PictureType Control

            if (data.PictureType != "User.ProfileBgPicture")
            {
                rtn = rtn.SendError(GlobalErrors.WrongPictureType);
                return Ok(rtn);
            }

            #endregion PictureType Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);

                var serviceFilterModel = new ServiceParamsWithIdentifier<ChangePictureModel>(data, userId, tokenId);
                IReturnModel<string> serviceAction = _service.ChangeBgPicture(serviceFilterModel);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Authorize]
        [Route("api/UserManagement/User/Get")]
        [HttpGet]
        public ActionResult<IReturnModel<UserDTO>> Get([FromQuery]ActionFilterModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<UserDTO> rtn = new ReturnModel<UserDTO>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:Get"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                IReturnModel<UserDTO> serviceAction = _service.FirstOrDefault(filter);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;

                if (rtn.Result != null)
                    rtn.Result.Password = string.Empty;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Authorize]
        [Route("api/UserManagement/User/UserListForCurrentUser")]
        [HttpGet]
        public ActionResult<IReturnModel<IList<UserForCurrentUser>>> UserListForCurrentUser()
        {
            #region Declares

            IReturnModel<IList<UserForCurrentUser>> rtn = new ReturnModel<IList<UserForCurrentUser>>(_logger);

            #endregion Declares

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                IReturnModel<IList<UserForCurrentUser>> serviceAction = _service.UserListForCurrentUser(new ServiceParamsWithIdentifier<int>(userId, userId, tokenId));
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;

                if (rtn.Result.Any())
                {
                    foreach (var result in rtn.Result)
                    {
                        result.Password = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Authorize]
        [Route("api/UserManagement/User/ChangeProfileInformations")]
        [HttpPost]
        public ActionResult<IReturnModel<bool>> ChangeProfileInformations([FromBody]ChangeProfileInformationsModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ChangeProfileInformations"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                var serviceFilterModel = new ServiceParamsWithIdentifier<ChangeProfileInformationsModel>(filter, userId, tokenId);
                IReturnModel<bool> serviceAction = _service.ChangeProfileInformations(serviceFilterModel);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Route("api/UserManagement/User/ListWithDetails")]
        [HttpGet]
        public ActionResult<IReturnModel<IList<UserDetailsModel>>> ListWithDetails([FromQuery]ActionFilterModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<IList<UserDetailsModel>> rtn = new ReturnModel<IList<UserDetailsModel>>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ListWithDetails"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                var serviceFilterModel = new ServiceParamsWithIdentifier<ActionFilterModel>(filter, userId, tokenId);
                IReturnModel<IList<UserDetailsModel>> serviceAction = _service.ListWithDetails(serviceFilterModel);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Authorize(Roles = "Root, Root.UserManagement, Root.UserManagement.User, Root.UserManagement.User.Add, Root.UserManagement.User.Update")]
        [Route("api/UserManagement/User/Save")]
        [HttpPost]
        public ActionResult<IReturnModel<UserDTO>> Save([FromBody]SaveUserModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<UserDTO> rtn = new ReturnModel<UserDTO>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:Save"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                var serviceFilterModel = new ServiceParamsWithIdentifier<SaveUserModel>(filter, userId, tokenId);
                IReturnModel<UserDTO> serviceAction = _service.Save(serviceFilterModel);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Authorize(Roles = "Root, Root.UserManagement, Root.UserManagement.User, Root.UserManagement.User.ChangeStatus")]
        [Route("api/UserManagement/User/ChangeStatus")]
        [HttpPost]
        public ActionResult<IReturnModel<bool>> ChangeStatus([FromBody]ChangeStatusModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ChangeStatus"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                var serviceFilterModel = new ServiceParamsWithIdentifier<ChangeStatusModel>(filter, userId, tokenId);
                IReturnModel<bool> serviceAction = _service.ChangeStatus(serviceFilterModel);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Authorize(Roles = "Root, Root.UserManagement, Root.UserManagement.User, Root.UserManagement.User.UserInGroup")]
        [Route("api/UserManagement/User/SaveUserInGroups")]
        [HttpPost]
        public ActionResult<IReturnModel<bool>> SaveUserInGroups([FromBody]SaveUserInGroupsModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:SaveUserInGroups"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                var serviceFilterModel = new ServiceParamsWithIdentifier<SaveUserInGroupsModel>(filter, userId, tokenId);
                IReturnModel<bool> serviceAction = _service.SaveUserInGroups(serviceFilterModel);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        [Authorize(Roles = "Root, Root.UserManagement, Root.UserManagement.User, Root.UserManagement.User.ChangeRoles")]
        [Route("api/UserManagement/User/SaveUserInRoles")]
        [HttpPost]
        public ActionResult<IReturnModel<bool>> SaveUserInRoles([FromBody]SaveUserInRolesModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:SaveUserInRoles"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                var serviceFilterModel = new ServiceParamsWithIdentifier<SaveUserInRolesModel>(filter, userId, tokenId);
                IReturnModel<bool> serviceAction = _service.SaveUserInRoles(serviceFilterModel);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;
            }
            catch (Exception ex)
            {
                rtn = rtn.SendError(GlobalErrors.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok(rtn);
        }

        #endregion Actions
    }
}