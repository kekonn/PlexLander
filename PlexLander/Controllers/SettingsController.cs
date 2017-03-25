using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlexLander.Configuration;
using PlexLander.Data;
using PlexLander.ViewModels.Settings;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlexLander.Controllers
{
    public class SettingsController : PlexLanderBaseController
    {
        private const string ControllerName = "Settings";

        public SettingsController(PlexLanderContext context, IOptions<ServerConfiguration> config) : base(context,config)
        { }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(new SettingsIndexViewModel(ServerName) { ActiveControllerName = ControllerName });
        }
    }
}
