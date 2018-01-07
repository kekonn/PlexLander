using PlexLander.ViewModels.Landing;
using Microsoft.AspNetCore.Mvc;
using PlexLander.Data;
using PlexLander.Configuration;
using PlexLander.ViewModels;
using System;

namespace PlexLander.Controllers
{
    public class LandingController : PlexLanderBaseController
    {
        private readonly IAppRepository _appRepo;

        public LandingController(IAppRepository appRepo, IConfigurationManager configManager) : base(configManager)
        {
            _appRepo = appRepo;
        }

        public IActionResult Index()
        {
            return View(new LandingViewModel(this.ServerName) { AppList = AppViewModelFactory.FromApps(userApps: _appRepo.ListAll(), builtInApps: ConfigManager.ListAll()) });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
