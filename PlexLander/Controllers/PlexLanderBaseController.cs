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
        private readonly ConfigurationManager _configManager;
        protected ConfigurationManager ConfigManager => _configManager;
        protected string ServerName {
            get
            {
                return _configManager.ServerName;
            }
        }
        
        public PlexLanderBaseController(ConfigurationManager configManager) : base()
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
