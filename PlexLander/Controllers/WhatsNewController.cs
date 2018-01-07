using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlexLander.Configuration;
using PlexLander.Data;
using PlexLander.ViewModels.WhatsNew;

namespace PlexLander.Controllers
{
    public class WhatsNewController : PlexLanderBaseController
    {
        private readonly IWhatsNewService _whatsNewService;

        public WhatsNewController(IWhatsNewService whatsNewService, IConfigurationManager configManager) : base(configManager)
        {
            _whatsNewService = whatsNewService;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel(ServerName));
        }
    }
}