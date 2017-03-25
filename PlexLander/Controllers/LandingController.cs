using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.ViewModels.Landing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlexLander.Data;
using PlexLander.Models;
using PlexLander.Configuration;

namespace PlexLander.Controllers
{
    public class LandingController : PlexLanderBaseController
    {
        public LandingController(PlexLanderContext context, IOptions<ServerConfiguration> config) : base(context, config)
        {
        }

        public IActionResult Index()
        {
            return View(new LandingViewModel(this.ServerName) { AppList = new List<App>(Context.Apps.AsEnumerable())});
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
