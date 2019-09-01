using AutoMapper;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Poco;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CSBEF.Module.UserManagement.Services
{
    public class RoleService : ServiceBase<Role, RoleDTO>, IRoleService
    {
        #region Dependencies



        #endregion

        #region ctor

        public RoleService(
           IHostingEnvironment hostingEnvironment,
           IConfiguration configuration,
           ILogger<ILog> logger,
           IMapper mapper,
           IRoleRepository repository,
           IEventService eventService

           // Other Repository Dependencies
        ) : base(
           hostingEnvironment,
           configuration,
           logger,
           mapper,
           repository,
           eventService,
           "UserManagement",
           "RoleService"
        )
        {
            
        }

        #endregion ctor

        #region Public Actions



        #endregion Public Actions
    }
}
