﻿using Microsoft.AspNetCore.Mvc;
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
        protected string ServerName { get; private set; }
        
        public PlexLanderBaseController(IOptions<ServerConfiguration> config) : base()
        {
            ServerName = config.Value.ServerName;
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
