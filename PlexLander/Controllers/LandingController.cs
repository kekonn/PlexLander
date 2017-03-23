using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.ViewModels.Landing;
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
            var uriBuilder = new UriBuilder()
            {
                Host = AppSettings.Hostname,
                Path = AppSettings.PlexPath,
                Scheme = AppSettings.Transport
            };
            return View(new LandingViewModel() { PlexUrl = uriBuilder.Uri });
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
