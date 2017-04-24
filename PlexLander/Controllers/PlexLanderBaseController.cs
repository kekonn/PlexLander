using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using PlexLander.Configuration;
using PlexLander.Data;
using PlexLander.ViewModels;
using System;

namespace PlexLander.Controllers
{
    public abstract class PlexLanderBaseController : Controller
    {
        private readonly IConfigurationManager _configManager;
        protected IConfigurationManager ConfigManager => _configManager;
        protected string ServerName {
            get
            {
                return _configManager.ServerName;
            }
        }
        
        public PlexLanderBaseController(IConfigurationManager configManager) : base()
        {
            _configManager = configManager;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as PlexLanderBaseController;

            if (controller != null && controller.ViewData.Model == null)
            {
               controller.ViewData.Model = new BasicViewModel(ServerName);
            }
            base.OnActionExecuted(context);
        }

        private sealed class BasicViewModel : ViewModelBase
        { 
            public BasicViewModel(string serverName) : base(serverName)
            {
            }
        }
    }
}
