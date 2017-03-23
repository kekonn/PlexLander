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

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
