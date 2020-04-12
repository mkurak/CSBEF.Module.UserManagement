using System;
using CSBEF.Core.Concretes;
using CSBEF.Core.Interfaces;
using CSBEF.Module.UserManagement.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CSBEF.Module.UserManagement {
    public class ModuleEventsJoinInitializer : IModuleEventsJoinInitializer {
        private readonly IServiceProvider _serviceProvicer;

        public ModuleEventsJoinInitializer (IServiceProvider serviceProvider) {
            _serviceProvicer = serviceProvider;
        }

        public void Start (IEventService eventService) {
            if (eventService == null)
                throw new ArgumentNullException (nameof (eventService));

            eventService.GetEvent ("Main", "InComingToken").EventEvent += MainInComingTokenHandler;
        }

        private dynamic MainInComingTokenHandler (dynamic token, IEventInfo eventInfo) {
            var tokenService = _serviceProvicer.GetService<ITokenService> ();

            var run = tokenService.CheckToken (new ServiceParamsWithIdentifier<string> (token as string, 0, 0));
            return run;
        }
    }
}