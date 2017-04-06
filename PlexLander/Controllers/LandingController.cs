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
        private readonly IAppRepository _appRepo;

        public LandingController(IAppRepository appRepo, IOptions<ServerConfiguration> config) : base(config)
        {
            _appRepo = appRepo;
        }

        public IActionResult Index()
        {
            return View(new LandingViewModel(this.ServerName) { AppList = new List<App>(_appRepo.ListAll())});
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
