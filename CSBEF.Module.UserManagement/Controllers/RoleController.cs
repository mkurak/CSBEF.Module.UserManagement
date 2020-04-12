using System;
using System.Collections.Generic;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CSBEF.Module.UserManagement.Controllers {
    [ApiController]
    public class RoleController : ControllerBase {
        #region Dependencies

        private readonly ILogger<IReturnModel<bool>> _logger;
        private readonly IRoleService _service;

        #endregion Dependencies

        #region Construction

        public RoleController (ILogger<IReturnModel<bool>> logger, IRoleService roleService) {
            _logger = logger;
            _service = roleService;
        }

        #endregion Construction

        #region Actions

        [Authorize]
        [Route ("api/UserManagement/Role/List")]
        [HttpGet]
        public ActionResult<IReturnModel<IList<RoleDTO>>> List ([FromQuery] ActionFilterModel filter) {
            if (filter == null)
                throw new ArgumentNullException (nameof (filter));

            #region Declares

            IReturnModel<IList<RoleDTO>> rtn = new ReturnModel<IList<RoleDTO>> (_logger);

            #endregion Declares

            #region Action Body

            try {
                var userId = Tools.GetTokenNameClaim (HttpContext);
                IReturnModel<IList<RoleDTO>> serviceAction = _service.List (filter);
                if (serviceAction.ErrorInfo.Status)
                    rtn.ErrorInfo = serviceAction.ErrorInfo;
                else
                    rtn.Result = serviceAction.Result;
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok (rtn);
        }

        #endregion Actions
    }
}