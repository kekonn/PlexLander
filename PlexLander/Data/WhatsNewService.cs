using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.Models;
using PlexLander.Data;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PlexLander.Data
{
    public interface IWhatsNewService
    {
    }

    public class WhatsNewService : IWhatsNewService
    {
        private readonly Plex.IPlexService _plexService;

        public WhatsNewService(Plex.IPlexService plexService)
        {
            _plexService = plexService ?? throw new ArgumentNullException("plexServer");
        }

        public IEnumerable<PlexServer> GetPlexServers()
        {
            throw new NotImplementedException();
        }
    }
}
