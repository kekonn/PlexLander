using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Plex
{
    public class PlexServer
    {
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Port { get; set; }
        public string Scheme { get; set; }
        public List<String> LocalAddresses { get; set; }
        public bool Owned { get; set; }
        public bool UsesPlexHome { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
