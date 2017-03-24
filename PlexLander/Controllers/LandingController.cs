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
        private readonly PlexLanderContext _context;

        public LandingController(PlexLanderContext context, IOptions<ServerConfiguration> config) : base(config)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(new LandingViewModel() { AppList = new List<App>(_context.Apps.AsEnumerable()), ServerName = this.ServerName });
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
