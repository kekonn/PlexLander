using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlexLander.Configuration;
using PlexLander.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlexLander.Controllers
{
    public class SettingsController : PlexLanderBaseController
    {
        public SettingsController(PlexLanderContext context, IOptions<ServerConfiguration> config) : base(context,config)
        { }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
