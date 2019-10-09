using AutoMapper;
using CSBEF.Core.Abstracts;
using CSBEF.Core.Interfaces;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Models.DTO;
using CSBEF.Module.UserManagement.Poco;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CSBEF.Module.UserManagement.Services
{
    public class RoleService : ServiceBase<Role, RoleDTO>, IRoleService
    {
        #region ctor

        public RoleService(
           IWebHostEnvironment hostingEnvironment,
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
    }
}