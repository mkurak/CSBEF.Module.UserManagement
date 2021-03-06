﻿using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
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

namespace CSBEF.Module.UserManagement.Controllers
{
    [ApiController]
    public class GroupController : ControllerBase
    {
        #region Dependencies

        private readonly IConfiguration _configuration;
        private readonly ILogger<ILog> _logger;
        private readonly IGroupService _service;

        #endregion Dependencies

        #region Construction

        public GroupController(IConfiguration configuration, ILogger<ILog> logger, IGroupService groupService)
        {
            _configuration = configuration;
            _logger = logger;
            _service = groupService;
        }

        #endregion Construction

        #region Actions

        [Authorize]
        [Route("api/UserManagement/Group/List")]
        [HttpGet]
        public ActionResult<IReturnModel<IList<GroupDTO>>> List([FromQuery]ActionFilterModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<IList<GroupDTO>> rtn = new ReturnModel<IList<GroupDTO>>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:GroupController:List"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                IReturnModel<IList<GroupDTO>> serviceAction = _service.List(filter);
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
        [Route("api/UserManagement/Group/ListWithDetails")]
        [HttpGet]
        public ActionResult<IReturnModel<IList<UserGroupDetailsModel>>> ListWithDetails([FromQuery]ActionFilterModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<IList<UserGroupDetailsModel>> rtn = new ReturnModel<IList<UserGroupDetailsModel>>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:GroupController:ListWithDetails"]))
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
                IReturnModel<IList<UserGroupDetailsModel>> serviceAction = _service.ListWithDetails(serviceFilterModel);
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

        [Authorize(Roles = "Root, Root.UserManagement, Root.UserManagement.Group, Root.UserManagement.Group.Add, Root.UserManagement.Group.Update")]
        [Route("api/UserManagement/Group/Save")]
        [HttpPost]
        public ActionResult<IReturnModel<GroupDTO>> Save([FromBody]SaveGroupModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<GroupDTO> rtn = new ReturnModel<GroupDTO>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:GroupController:Save"]))
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
                var serviceFilterModel = new ServiceParamsWithIdentifier<SaveGroupModel>(filter, userId, tokenId);
                IReturnModel<GroupDTO> serviceAction = _service.Save(serviceFilterModel);
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

        [Authorize(Roles = "Root, Root.UserManagement, Root.UserManagement.Group, Root.UserManagement.Group.ChangeStatus")]
        [Route("api/UserManagement/Group/ChangeStatus")]
        [HttpPost]
        public ActionResult<IReturnModel<bool>> ChangeStatus([FromBody]ChangeStatusModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:GroupController:ChangeStatus"]))
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

        [Authorize(Roles = "Root, Root.GroupManagement, Root.GroupManagement.Group, Root.GroupManagement.Group.ChangeRoles")]
        [Route("api/UserManagement/Group/SaveGroupInRoles")]
        [HttpPost]
        public ActionResult<IReturnModel<bool>> SaveGroupInRoles([FromBody]SaveGroupInRoleModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<bool> rtn = new ReturnModel<bool>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:GroupController:SaveGroupInRoles"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var GroupId = Tools.GetTokenNameClaim(HttpContext);
                var tokenId = Tools.GetTokenIdClaim(HttpContext);
                var serviceFilterModel = new ServiceParamsWithIdentifier<SaveGroupInRoleModel>(filter, GroupId, tokenId);
                IReturnModel<bool> serviceAction = _service.SaveGroupInRoles(serviceFilterModel);
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