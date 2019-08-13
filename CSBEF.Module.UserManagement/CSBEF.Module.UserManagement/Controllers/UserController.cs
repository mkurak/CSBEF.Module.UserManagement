using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Models.Request;
using CSBEF.Module.UserManagement.Models.Return;
using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Core.Models.HelperModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IReturnModel<bool>>> ChangePassword([FromBody]ChangePasswordModel data)
        {
            #region Declares

            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            #endregion Declares

            #region Hash Control

            if (!data.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ChangePassword"]))
            {
                _logger.LogError("InvalidHash: " + data.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);

                var serviceFilterModel = new ServiceParamsWithIdentifier<ChangePasswordModel>(data, userId, tokenId);
                IReturnModel<bool> serviceAction = await _service.ChangePassword(serviceFilterModel);
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
        public async Task<ActionResult<IReturnModel<string>>> ChangeProfilePicture([FromForm]ChangePictureModel data)
        {
            #region Declares

            IReturnModel<string> rtn = new ReturnModel<string>(_logger);

            #endregion Declares

            #region Hash Control

            if (!data.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ChangeProfilePicture"]))
            {
                _logger.LogError("InvalidHash: " + data.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion

            #region PictureType Control

            if(data.PictureType != "User.ProfilePicture")
            {
                rtn = rtn.SendError(GlobalErrors.WrongPictureType);
                return Ok(rtn);
            }

            #endregion

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);

                var serviceFilterModel = new ServiceParamsWithIdentifier<ChangePictureModel>(data, userId, tokenId);
                IReturnModel<string> serviceAction = await _service.ChangePicture(serviceFilterModel);
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
        public async Task<ActionResult<IReturnModel<string>>> ChangeProfileBgPicture([FromForm]ChangePictureModel data)
        {
            #region Declares

            IReturnModel<string> rtn = new ReturnModel<string>(_logger);

            #endregion Declares

            #region Hash Control

            if (!data.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ChangeProfileBgPicture"]))
            {
                _logger.LogError("InvalidHash: " + data.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion

            #region PictureType Control

            if (data.PictureType != "User.ProfileBgPicture")
            {
                rtn = rtn.SendError(GlobalErrors.WrongPictureType);
                return Ok(rtn);
            }

            #endregion

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);

                var serviceFilterModel = new ServiceParamsWithIdentifier<ChangePictureModel>(data, userId, tokenId);
                IReturnModel<string> serviceAction = await _service.ChangeBgPicture(serviceFilterModel);
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
        public async Task<ActionResult<IReturnModel<UserDTO>>> Get([FromQuery]ActionFilterModel filter)
        {
            #region Declares

            IReturnModel<UserDTO> rtn = new ReturnModel<UserDTO>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:Get"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                IReturnModel<UserDTO> serviceAction = await _service.FirstOrDefaultAsync(filter);
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;

                if (rtn.Result != null)
                    rtn.Result.Password = "";
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
        public async Task<ActionResult<IReturnModel<IList<UserForCurrentUser>>>> UserListForCurrentUser()
        {
            #region Declares

            IReturnModel<IList<UserForCurrentUser>> rtn = new ReturnModel<IList<UserForCurrentUser>>(_logger);

            #endregion Declares

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                IReturnModel<IList<UserForCurrentUser>> serviceAction = await _service.UserListForCurrentUser(new ServiceParamsWithIdentifier<int>(userId, userId, tokenId));
                if (serviceAction.Error.Status)
                    rtn.Error = serviceAction.Error;
                else
                    rtn.Result = serviceAction.Result;

                if (rtn.Result.Any())
                {
                    foreach (var result in rtn.Result)
                    {
                        result.Password = "";
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
        public async Task<ActionResult<IReturnModel<bool>>> ChangeProfileInformations([FromBody]ChangeProfileInformationsModel filter)
        {
            #region Declares

            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:UserController:ChangeProfileInformations"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                var serviceFilterModel = new ServiceParamsWithIdentifier<ChangeProfileInformationsModel>(filter, userId, tokenId);
                IReturnModel<bool> serviceAction = await _service.ChangeProfileInformations(serviceFilterModel);
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