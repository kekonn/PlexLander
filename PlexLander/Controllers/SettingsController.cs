using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlexLander.Configuration;
using PlexLander.Data;
using PlexLander.ViewModels.Settings;
using PlexLander.Models;
using System.Collections.Generic;

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
            var apps = new List<App>(Context.Apps.AsEnumerable());
            return View(new SettingsIndexViewModel(ServerName) { ActiveControllerName = ControllerName, Apps = apps });
        }
    }
}
