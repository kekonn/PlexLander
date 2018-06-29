using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlexLander.Configuration;
using PlexLander.Data;
using PlexLander.Plex;
using PlexLander.Mapping;
using PlexLander.ViewModels;
using PlexLander.ViewModels.WhatsNew;

namespace PlexLander.Controllers
{
    public class WhatsNewController : PlexLanderBaseController
    {
        private readonly IWhatsNewService _whatsNewService;
        private readonly IPlexService _plexService;

        public WhatsNewController(IPlexService plexService, IWhatsNewService whatsNewService, IConfigurationManager configManager) : base(configManager)
        {
            _whatsNewService = whatsNewService;
            _plexService = plexService;
        }

        public async Task<IActionResult> Index(string server = null)
        {
            var plexServers = new List<PlexServer>(await _plexService.GetPlexServerAsync()).OrderBy(s => s.Name);
            var vm = new IndexViewModel(ServerName) { Servers = new List<PlexServerViewModel>(plexServers.MapToViewModel()) };

            PlexServer selectedServer = null;
            if (!string.IsNullOrWhiteSpace(server))
            {
                selectedServer = plexServers.SingleOrDefault(s => s.GetUri().Equals(server, StringComparison.InvariantCultureIgnoreCase));
                if (selectedServer == null)
                {
                    selectedServer = plexServers.FirstOrDefault(s => s.Owned);
                }
                vm.SelectedServer = selectedServer.MapToViewModel();
            }
            

            _plexService.GetRecentlyAddedAsync(selectedServer ?? plexServers.First());

            return View(vm);
        }
    }
}