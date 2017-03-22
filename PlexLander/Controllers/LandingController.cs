using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PlexLander.Controllers
{
    public class LandingController : PlexLanderBaseController
    {
        public LandingController(IOptions<AppSettings> options) : base(options)
        {
        }

        public IActionResult Index()
        {
            ViewData["PlexUrl"] = AppSettings.PlexUrl;
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
