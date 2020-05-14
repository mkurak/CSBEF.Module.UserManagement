using CSBEF.Core.Interfaces;
using CSBEF.Module.UserManagement.Interfaces.Repository;
using CSBEF.Module.UserManagement.Interfaces.Service;
using CSBEF.Module.UserManagement.Repositories;
using CSBEF.Module.UserManagement.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CSBEF.Module.UserManagement
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Init(IServiceCollection services)
        {
            #region Repositories

            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IGroupInRoleRepository, GroupInRoleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserInGroupRepository, UserInGroupRepository>();
            services.AddScoped<IUserInRoleRepository, UserInRoleRepository>();
            services.AddScoped<IUserMessageRepository, UserMessageRepository>();

            #endregion Repositories

            #region Services

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserMessageService, UserMessageService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IGroupService, GroupService>();

            #endregion Services
        }
    }
}