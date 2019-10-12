using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        #region Dependencies

        private readonly IConfiguration _configuration;
        private readonly ILogger<ILog> _logger;
        private readonly IRoleService _service;

        #endregion Dependencies

        #region Construction

        public RoleController(IConfiguration configuration, ILogger<ILog> logger, IRoleService roleService)
        {
            _configuration = configuration;
            _logger = logger;
            _service = roleService;
        }

        #endregion Construction

        #region Actions

        [Authorize]
        [Route("api/UserManagement/Role/List")]
        [HttpGet]
        public ActionResult<IReturnModel<IList<RoleDTO>>> List([FromQuery]ActionFilterModel filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            #region Declares

            IReturnModel<IList<RoleDTO>> rtn = new ReturnModel<IList<RoleDTO>>(_logger);

            #endregion Declares

            #region Hash Control

            if (!filter.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:RoleController:List"]))
            {
                _logger.LogError("InvalidHash: " + filter.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                var userId = Tools.GetTokenNameClaim(HttpContext);
                IReturnModel<IList<RoleDTO>> serviceAction = _service.List(filter);
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