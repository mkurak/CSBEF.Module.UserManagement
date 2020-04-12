using System;
using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CSBEF.Module.UserManagement.Controllers {
    [ApiController]
    public class TokenController : ControllerBase {
        #region Dependencies

        private readonly ILogger<IReturnModel<bool>> _logger;
        private readonly ITokenService _service;

        #endregion Dependencies

        #region Construction

        public TokenController (ILogger<IReturnModel<bool>> logger, ITokenService tokenService) {
            _logger = logger;
            _service = tokenService;
        }

        #endregion Construction

        #region Actions

        [Route ("api/UserManagement/Token/Test")]
        [HttpGet]
        [Authorize]
        public ActionResult<IReturnModel<bool>> Test () {
            return Ok (new ReturnModel<bool> (_logger).SendResult (true));
        }

        [Route ("api/UserManagement/Token/CreateToken")]
        [HttpPost]
        public ActionResult<IReturnModel<string>> CreateToken ([FromBody] CreateTokenModel data) {
            if (data == null)
                throw new ArgumentNullException (nameof (data));

            #region Declares

            IReturnModel<string> rtn = new ReturnModel<string> (_logger);

            #endregion Declares

            #region Action Body

            try {
                IReturnModel<string> getCreateToken = _service.CreateToken (new ServiceParamsWithIdentifier<CreateTokenModel> (data, 0, 0));
                if (getCreateToken.ErrorInfo.Status)
                    rtn.ErrorInfo = getCreateToken.ErrorInfo;
                else
                    rtn.Result = getCreateToken.Result;
            } catch (CustomException ex) {
                rtn = rtn.SendError (GlobalError.TechnicalError, ex);
            }

            #endregion Action Body

            return Ok (rtn);
        }

        #endregion Actions
    }
}