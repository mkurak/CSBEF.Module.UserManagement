using CSBEF.Core.Concretes;
using CSBEF.Core.Enums;
using CSBEF.Core.Helpers;
using CSBEF.Core.Interfaces;
using CSBEF.Core.Models;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CSBEF.Module.UserManagement.Controllers
{
    [ApiController]
    public class TokenController : ControllerBase
    {
        #region Dependencies

        private IConfiguration _configuration;
        private readonly ILogger<ILog> _logger;
        private readonly ITokenService _service;

        #endregion Dependencies

        #region Construction

        public TokenController(IConfiguration configuration, ILogger<ILog> logger, ITokenService tokenService)
        {
            _configuration = configuration;
            _logger = logger;
            _service = tokenService;
        }

        #endregion Construction

        #region Actions

        [Route("api/UserManagement/Token/Test")]
        [HttpGet]
        [Authorize]
        public ActionResult<IReturnModel<bool>> Test()
        {
            return Ok(new ReturnModel<bool>(_logger).SendResult(true));
        }

        [Route("api/UserManagement/Token/CreateToken")]
        [HttpPost]
        public async Task<ActionResult<IReturnModel<string>>> CreateToken([FromBody]CreateTokenModel data)
        {
            #region Declares

            IReturnModel<string> rtn = new ReturnModel<string>(_logger);

            #endregion Declares

            #region Hash Control

            if (!data.HashControl(_configuration["AppSettings:HashSecureKeys:UserManagement:TokenController:CreateToken"]))
            {
                _logger.LogError("InvalidHash: " + data.Hash);
                return BadRequest(rtn.SendError(GlobalErrors.HashCodeInValid));
            }

            #endregion Hash Control

            #region Action Body

            try
            {
                IReturnModel<string> getCreateToken = await _service.CreateToken(new ServiceParamsWithIdentifier<CreateTokenModel>(data, 0, 0));
                if (getCreateToken.Error.Status)
                    rtn.Error = getCreateToken.Error;
                else
                    rtn.Result = getCreateToken.Result;
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